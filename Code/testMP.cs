using Data;
using Godot;
using System;

public partial class testMP : Node
{
	public override void _Ready()
	{
        AutoLoad.auto.mundo = GetNode<Node3D>("Mundo");
	}

	public void _on_connect_pressed()
	{
		AdminNet.admin.StartClient(GetNode<LineEdit>("Interfaz/IP").Text);
		Player player = new Player();
        player.pos=GetNode<Node3D>("Mundo/Spawn").Position;
		player.name = GetNode<LineEdit>("Interfaz/Name").Text;
        AdminNet.admin.cli.Send(player);
        GetNode<Control>("Interfaz").Visible = false;
    }
    public void _on_server_pressed()
    {
        AdminNet.admin.StartServer();
		Player player = new Player();
        player.pos=GetNode<Node3D>("Mundo/Spawn").Position;
		player.name = GetNode<LineEdit>("Interfaz/Name").Text;
        AdminNet.admin.cli.Send(player);
        GetNode<Control>("Interfaz").Visible = false;
    }
}