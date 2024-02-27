using Data;
using Godot;
using System;

public partial class LocalTorreta : Node3D
{
	public Node3D Base, Pie, Cabeza;
	public Vector3 Punto;
    public Torreta torreta;
	PackedScene Bala;
    int countBalas=0;
    double timeSend = 0;
	[Export]
	int id = 0;
	public override void _Ready()
	{
		Base = GetNode<Node3D>("Base");
        Pie = Base.GetNode<Node3D>("Pie");
        Cabeza = Pie.GetNode<Node3D>("Cabeza");
        Bala = ResourceLoader.Load<PackedScene>("res://bala.tscn");
        torreta = new Torreta();
        torreta.tid = id;
    }
    public override void _Process(double delta)
    {
        if (AdminNet.admin.cli != null && torreta.IDPlayer != 0 && torreta.IDPlayer != AdminNet.admin.cli.MyID)
        {
            Base.GlobalRotation = Vector3.Up * torreta.rot.X;
            Pie.GlobalRotation = Vector3.Right * torreta.rot.Y;
        }
        if (AdminNet.admin.cli != null && torreta.IDPlayer == AdminNet.admin.cli.MyID)
        {
            if (AdminNet.admin.start)
            {
                if (timeSend > 0)
                    timeSend -= delta * 4;
                else
                {
                    timeSend = 1;
                    torreta.rot = new Vector2(Base.GlobalRotation.Y, Pie.GlobalRotation.X);
                    AdminNet.admin.cli.Send(torreta);
                }
            }
        }
    }
    public void Disparar()
	{
		var b=Bala.Instantiate<LocalBala>();
        countBalas++;
        b.bala.bid = int.Parse(countBalas.ToString()+torreta.IDPlayer.ToString());//maximo 10 players
		b.Position = Cabeza.GlobalPosition;
		b.Rotation = Cabeza.GlobalRotation;
        AutoLoad.auto.mundo.AddChild(b);

    }
    public void _on__body_entered(Node3D node)
    {
        if (node is LocalPlayer lp)
        {
            lp.area = id;
        }
    }
    public void _on__body_exited(Node3D node)
    {
        if (node is LocalPlayer lp)
        {
            lp.area = 0;
        }
    }
}
