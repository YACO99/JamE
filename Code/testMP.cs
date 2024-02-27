using Data;
using Godot;
using System;
using System.Threading;

public partial class testMP : Node
{
	public override void _Ready()
	{
		AutoLoad.auto.mundo = GetNode<Node3D>("Mundo");
        for (int i = 1; i <= 3; i++)
        {
            AutoLoad.auto.torretas.Add(GetNode<LocalTorreta>("Mundo/"+ i +"/Torreta"));
        }
    }

	public void _on_connect_pressed()
	{
		AdminNet.admin.StartClient(GetNode<LineEdit>("Interfaz/IP").Text);
		Player player = new Player();
		player.pos = GetNode<Node3D>("Mundo/Spawn").Position;
		player.name = GetNode<LineEdit>("Interfaz/Name").Text;
		AdminNet.admin.cli.Send(player);
		GetNode<Control>("Interfaz").Visible = false;
	}
	public void _on_server_pressed()
	{
		AdminNet.admin.StartServer();
		Player player = new Player();
		player.pos = GetNode<Node3D>("Mundo/Spawn").Position;
		player.name = GetNode<LineEdit>("Interfaz/Name").Text;
		AdminNet.admin.cli.Send(player);
		GetNode<Control>("Interfaz").Visible = false;
		var u = new Upnp();
		try
		{
			//u.GetDevice(u.Discover()).AddPortMapping(41858, 41858, "juegoJam", "TCP", 1800);
		}
		catch (Exception)
		{
			GD.PrintErr("NO UPNP");
		}
	}
}
