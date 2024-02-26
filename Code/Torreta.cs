using Godot;
using System;

public partial class Torreta : Node3D
{
	public Node3D Base, Pie, Cabeza;
	public Vector3 Punto;
	PackedScene Bala;
	public override void _Ready()
	{
		Base = GetNode<Node3D>("Base");
        Pie = Base.GetNode<Node3D>("Pie");
        Cabeza = Pie.GetNode<Node3D>("Cabeza");
        Bala = ResourceLoader.Load<PackedScene>("res://bala.tscn");
    }

	public void Disparar()
	{
		var b=Bala.Instantiate<Area3D>();//crear bala
		AutoLoad.auto.mundo.AddChild(b);
		b.GlobalPosition = Cabeza.GlobalPosition;
		b.GlobalRotation = Cabeza.GlobalRotation;
	}
}
