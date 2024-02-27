using Data;
using Godot;
using System;

public partial class LocalRoca : RigidBody3D
{
	public Roca roca;
    double timeSend = 0;
    public override void _Process(double delta)
	{
        if (AdminNet.admin.ser != null)
        {
            if (AdminNet.admin.start)
            {
                if (timeSend > 0)
                    timeSend -= delta * 4;
                else
                {
                    timeSend = 1;
                    roca.pos = Position;
                    roca.vel = LinearVelocity;
                    AdminNet.admin.cli.Send(roca);
                }
            }
        }
        else
        {
            Position += (roca.pos - Position) * (float)delta * 5;
            LinearVelocity = roca.vel;
        }
    }
}
