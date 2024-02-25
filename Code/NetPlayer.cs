using Data;
using Godot;
using System;

public partial class NetPlayer : CharacterBody3D
{
	public Player player;

	public override void _PhysicsProcess(double delta)
	{
	    Position = player.pos;
        GetNode<Label3D>("Name").Text = player.name;
        Velocity = player.vel;
        var temp = (player.ry - GlobalRotationDegrees.Y);
        if (temp < -180)
            temp += 360;
        if (temp > 180)
            temp -= 360;
        GlobalRotationDegrees += temp * Vector3.Up * (float)delta * 10;
        MoveAndSlide();
	}
}
