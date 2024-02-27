using Data;
using Godot;
using System;
using System.Collections.Generic;

public partial class AutoLoad : Node
{

	public static AutoLoad auto { get; set; }
	public AdminNet admin = new AdminNet();
	List<NetPlayer> players = new List<NetPlayer>();
	List<LocalBala> balas = new List<LocalBala>();
	List<LocalRoca> rocas = new List<LocalRoca>();
	public List<LocalTorreta> torretas = new List<LocalTorreta>();
	LocalPlayer lPlayer;
	public Node3D mundo;
	PackedScene NP,NB,NT,NR,LP;
    public override void _Ready()
	{
		auto = this;
		admin.Start();
		NP = ResourceLoader.Load<PackedScene>("res://NetPlayer.tscn");
		NB = ResourceLoader.Load<PackedScene>("res://bala.tscn");
		NR = ResourceLoader.Load<PackedScene>("res://Roca.tscn");
		LP = ResourceLoader.Load<PackedScene>("res://LocalPlayer.tscn");

    }

	public override void _Process(double delta)
	{
		admin.Update();
		if (admin.cli != null)
		{
			for (int i = 0; i < admin.cli.escena.players.Count; i++)
			{
				if (admin.cli.escena.players[i].id != admin.cli.MyID)
				{
					var np = players.Find(x => x.player.id == admin.cli.escena.players[i].id);
					if (np != null)
					{
						np.player = admin.cli.escena.players[i];
					}
					else
					{
						var temp = NP.Instantiate<NetPlayer>();
						temp.player = admin.cli.escena.players[i];
						temp.Position = temp.player.pos;
						players.Add(temp);
                        mundo.AddChild(temp);
					}
				}
				else if(lPlayer == null)
				{
                    lPlayer = LP.Instantiate<LocalPlayer>();
					lPlayer.player = admin.cli.escena.players[i];
                    lPlayer.Position = lPlayer.player.pos;
                    mundo.AddChild(lPlayer);
                }
			}
			for (int i = 0; i < admin.cli.escena.rocas.Count; i++)
			{
                if (admin.ser == null)
                {
                    var nr = rocas.Find(x => x.roca.rid == admin.cli.escena.rocas[i].rid);
                    if (nr != null)
                    {
                        nr.roca = admin.cli.escena.rocas[i];
                    }
                    else
                    {
                        var temp = NR.Instantiate<LocalRoca>();
                        temp.roca = admin.cli.escena.rocas[i];
                        temp.Position = temp.roca.pos;
                        temp.LinearVelocity = temp.roca.vel;
                        rocas.Add(temp);
                        mundo.AddChild(temp);
                    }
                }
            }
            for (int i = 0; i < admin.cli.escena.balas.Count; i++)
            {
                if (admin.cli.escena.balas[i].id != admin.cli.MyID)
                {
                    var nb = balas.Find(x => x.bala.bid == admin.cli.escena.balas[i].bid);
                    if (nb != null)
                    {
                        nb.bala = admin.cli.escena.balas[i];
                    }
                    else
                    {
                        var temp = NB.Instantiate<LocalBala>();
                        temp.bala = admin.cli.escena.balas[i];
                        temp.Position = temp.bala.pos;
                        temp.RotationDegrees = temp.bala.rot;
                        balas.Add(temp);
                        mundo.AddChild(temp);
                    }
                }
            }
            for (int i = 0; i < admin.cli.escena.torretas.Count; i++)
            {
                if (admin.cli.escena.torretas[i].id != admin.cli.MyID)
                {
                    var nt = torretas.Find(x => x.torreta.tid == admin.cli.escena.torretas[i].tid);
                    if (nt != null)
                    {
                        nt.torreta = admin.cli.escena.torretas[i];
                    }
                }
            }
            //chat=admin.cli.escena.Chat;
        }
    }
}
