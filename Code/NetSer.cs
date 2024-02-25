using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Data
{
	public partial class NetSer
	{
		List<Client> Clients = new List<Client>();
		List<Pack> datos = new List<Pack>();
		Thread t1, t2, t3;
		bool start = false;
		TcpListener listener;
		int ID = 1, MyID;
		Escena escena = new Escena();
		public void SetIDAdmin(int id)
		{
			MyID = id;
        }
		public void Start()
		{
			listener = new TcpListener(System.Net.IPAddress.Any, 41858);
			t1 = new Thread(new ThreadStart(delegate ()
			{
				while (start)
				{
					Client c = new Client();
					c.id = ID++;
					c.tcp = listener.AcceptTcpClient();
					c.tcp.ReceiveTimeout = 10;
					c.tcp.GetStream().Write(BitConverter.GetBytes(c.id));
					Clients.Add(c);
				}
			}));
			t2 = new Thread(new ThreadStart(delegate ()
			{
				while (start)
				{
					for (int i = 0; i < Clients.Count; i++)
					{//try
						byte[] b = new byte[Pack.size];
						Clients[i].tcp.GetStream().Read(b);
						datos.Add(Pack.SetBytes(b));
					}
				}
			}));
			t3 = new Thread(new ThreadStart(delegate ()
			{
				while (start)
				{
					if (escena.players.Count > 1)
					{
						for (int i = 0; i < Clients.Count; i++)
						{
							byte[] b = new byte[Pack.size];
							Escena.GetBytes(escena).CopyTo(b, 0);
							Clients[i].tcp.GetStream().Write(b);
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
					if (p.id != MyID)
					{
						if (_p != null)
						{
							_p.set(p.name, p.px, p.py, p.pz, p.ry);
						}
						else
						{
							escena.players.Add(p);
						}
					}
				}
				else if (datos[0] is Roca r)
				{
					var _r = escena.rocas.Find(x => x.id == r.id);
					if (_r != null)
					{
						_r.set(r.px, r.py, r.pz);
					}
					else
					{
						escena.rocas.Add(r);
					}
				}
				else if (datos[0] is Chat c)
				{
					escena.Chat +="\n"+ c.chat;
				}
				datos.RemoveAt(0);
			}
		}
	}
	public class Client
	{
		public int id;
		public TcpClient tcp = null;
	}
}