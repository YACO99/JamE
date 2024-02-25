using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Godot;
using System.Linq;
using Data;
public partial class NetSer
{
	List<Client> Clients = new List<Client>();
	List<Pack> datos = new List<Pack>();
	Thread t1, t2, t3;
	bool start = false;
	TcpListener listener;
	int ID = 0, MyID;
	Escena escena = new Escena();
	public void SetIDAdmin(int id)
	{
		MyID = id;
	}
	public void Start()
	{
		listener = new TcpListener(System.Net.IPAddress.Any, 41858);
		listener.Server.ReceiveTimeout = -1;
		t1 = new Thread(new ThreadStart(delegate ()
		{
			while (start)
			{
				try
				{
					Client c = new Client();
					ID++;
					c.id = ID;
					c.tcp = listener.AcceptSocket();
					c.tcp.ReceiveTimeout = 10;
					c.tcp.Send(BitConverter.GetBytes(c.id));
					Clients.Add(c);
				}
				catch (Exception) { }
			}
		}));
		t2 = new Thread(new ThreadStart(delegate ()
		{
			while (start)
			{
				for (int i = 0; i < Clients.Count; i++)
				{
					try
					{
						byte[] b = new byte[Pack.size];
						Clients[i].tcp.Receive(b);
						datos.Add(Pack.SetBytes(b));
					}
					catch (Exception)
					{
					}
				}
			}
		}));
		t3 = new Thread(new ThreadStart(delegate ()
		{
			while (start)
			{
				if (escena.players.Count > 0)
				{
					for (int i = 0; i < Clients.Count; i++)
					{
						try
						{
							byte[] b = Escena.GetBytes(escena);
							Clients[i].tcp.Send(b);
						}
						catch (Exception) { }
					}
				}
				Thread.Sleep(100);
			}
		}));
		start = true;
		listener.Start();
		t1.Start();
		t2.Start();
		t3.Start();
	}

	public void Update()
	{
		if (datos.Count > 0)
		{
			if (datos[0] is Player p)
			{
				var _p = escena.players.Find(x => x.id == p.id);
				if (_p != null)
				{
					_p.set(p.name, p.pos, p.vel, p.ry);
				}
				else
				{
					escena.players.Add(p);
				}
			}
			else if (datos[0] is Roca r)
			{
				var _r = escena.rocas.Find(x => x.id == r.id);
				if (_r != null)
				{
					_r.set(r.pos, r.vel);
				}
				else
				{
					escena.rocas.Add(r);
				}
			}
			else if (datos[0] is Chat c)
			{
				escena.Chat += "\n" + c.chat;
			}
			datos.RemoveAt(0);
		}
	}
}
public class Client
{
	public int id;
	public Socket tcp = null;
}
