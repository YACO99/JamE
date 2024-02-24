using Data;
using Godot;
using System;

public partial class AutoLoad : Node
{

	public static AutoLoad auto { get; set; }
	public AdminNet admin = new AdminNet();
	public override void _Ready()
	{
		auto = this;
		admin.Start();
    }

	public override void _Process(double delta)
	{
		admin.Update();
		for (int i = 0; i < admin.cli.escena.players.Count; i++)
		{
		
		}
        for (int i = 0; i < admin.cli.escena.rocas.Count; i++)
        {

        }
		//chat=admin.cli.escena.Chat;
    }
}
