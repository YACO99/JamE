using Godot;
using System;
using Data;

public partial class LocalBala : Area3D
{
	double time=5,timeSend=0;
	public Bala bala=new Bala();

	public void _on_body_entered(Node3D node) {
        var bid = bala.bid.ToString();
        var id=AdminNet.admin.cli.MyID.ToString();
        if (int.Parse(bid.Substring(bid.Length - 1, 1)) == int.Parse(id.Substring(id.Length - 1, 1)))
            return;
        if (node is LocalRoca lr) 
        {
            lr.QueueFree();
            QueueFree();
            //net delete
        }
	}

    public override void _Process(double delta)
	{
        time -= delta;
        if (time <= 0)
        {
            QueueFree();
            //net delete
        }
        var bid = bala.bid.ToString();
        var id = AdminNet.admin.cli.MyID.ToString();
        if (int.Parse(bid.Substring(bid.Length - 1, 1)) != int.Parse(id.Substring(id.Length - 1, 1)))
            Position += (bala.pos - Position) * (float)delta * 10;
        else
        {
            if (AdminNet.admin.start)
            {
                if (timeSend > 0)
                    timeSend -= delta * 4;
                else
                {
                    timeSend = 1;
                    bala.pos = Position;
                    bala.rot = RotationDegrees;
                    AdminNet.admin.cli.Send(bala);
                }
            }
            Position += GlobalBasis.Z * (float)delta * 15f;
        }
	}
}
