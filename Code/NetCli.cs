using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Data;
using System.Net;
using Godot;

public partial class NetCli
{
	List<Pack> datos = new List<Pack>();
	Thread t1;
	Socket cliente;
	public int MyID = 0;
	public Escena escena=new Escena();
    public void Start(string ip, bool server = false)
	{
		cliente = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        cliente.ReceiveTimeout = 100;
        cliente.Connect(new IPEndPoint(IPAddress.Parse(ip), 41858));
		byte[] b = new byte[4];
		MyID = cliente.Receive(b);
        t1 = new Thread(new ThreadStart(delegate () { 
			while (cliente.Connected)
			{
				try
				{
					byte[] b = new byte[Pack.size];
					cliente.Receive(b);
					datos.Add(Pack.SetBytes(b));
				}
				catch (Exception) { }
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
	public void Send(Pack p) {
        try
        {
            p.id = MyID;
            byte[] b = Pack.GetBytes(p);
			cliente.Send(b);
        }
        catch (Exception e) {
			GD.Print(e.Message);
		}
    }
}