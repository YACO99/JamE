using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Data;
using System.Net;

public partial class NetCli
{
	List<Pack> datos = new List<Pack>();
	Thread t1;
	TcpClient cliente;
	int MyID = 0;
	public Escena escena=new Escena();
    public void Start(string ip, bool server = false)
	{
		cliente = new TcpClient();
        cliente.ReceiveTimeout = 10;
        cliente.Connect(new IPEndPoint(IPAddress.Parse(ip), 41858));
		byte[] b = new byte[4];
		MyID = cliente.GetStream().Read(b, 0, b.Length);
        t1 = new Thread(new ThreadStart(delegate () { 
			while (cliente.Connected)
			{
                byte[] b = new byte[Pack.size];
                cliente.GetStream().Read(b);
                datos.Add(Pack.SetBytes(b));
            }
		}));
		t1.Start();
		if (server)
			AdminNet.admin.ser.SetIDAdmin(MyID);
    }
	public void Update ()
	{
		if (datos.Count > 0) {
			if (datos[0] is Escena e)
			{
				escena = e;
			}
            datos.RemoveAt(0);
        }
	}
}
public class Client 
{
	public int id;
	public TcpClient tcp;
}