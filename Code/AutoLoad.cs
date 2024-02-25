using Data;
using Godot;
using System;
using System.Collections.Generic;

public partial class AutoLoad : Node
{

	public static AutoLoad auto { get; set; }
	public AdminNet admin = new AdminNet();
	List<NetPlayer> players = new List<NetPlayer>();
	LocalPlayer lPlayer;
	public Node3D mundo;
	PackedScene NP,LP;
    public override void _Ready()
	{
		auto = this;
		admin.Start();
		NP = ResourceLoader.Load<PackedScene>("res://NetPlayer.tscn");
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
						temp.Position = admin.cli.escena.players[i].pos;
                        mundo.AddChild(temp);
					}
				}
				else if(lPlayer == null)
				{
                    lPlayer = LP.Instantiate<LocalPlayer>();
					lPlayer.player = admin.cli.escena.players[i];
                    lPlayer.Position = admin.cli.escena.players[i].pos;
                    mundo.AddChild(lPlayer);
                }
			}
			for (int i = 0; i < admin.cli.escena.rocas.Count; i++)
			{

			}
			//chat=admin.cli.escena.Chat;
		}
    }
}
