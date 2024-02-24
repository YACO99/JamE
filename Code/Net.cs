using Godot;
using System;
using System.Threading;
using Data;
using System.Net.Sockets;

public partial class Net : Node
{
	List<Client> Clients = new List<Client>();
	Thread t1, t2;
	bool start = false;
	TcpListener listener;
	int ID = 1;
    public override void _Ready()
	{
		listener = new TcpListener(System.Net.IPAddress.Any,41858);
        t1 = new Thread(new ThreadStart(delegate () { 
			while (start)
			{
				Client c = new Client();
				c.id=ID++;
				c.tcp=listener.AcceptTcpClient();
				c.tcp.ReceiveTimeout = 10;
				c.tcp.GetStream().Write(BitConverter.GetBytes(c.id));
				Clients.Add(c);
            }
		}));
        t2 = new Thread(new ThreadStart(delegate () {
            while (start)
            {
                
            }
        }));
		start = true;
        listener.Start();
		t1.Start();
    }

	public override void _Process(double delta)
	{
	}
}
public class Client 
{
    public int id = 0;
	public Player player;
	public TcpClient tcp;
}