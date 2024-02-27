using Data;
using Godot;
using System;

public partial class LocalPlayer : CharacterBody3D
{
	const float Speed = 5.0f;
	const float JumpVelocity = 4.5f;
	Camera camera;
	Vector3 velocity;
	public Player player;
	double timeSend = 0;
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public bool Disparando = false;
	public LocalTorreta Torreta = null;
	public int area=0;
    public override void _Ready()
	{
		camera = GetNode<Camera>("Camera");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;


		if (IsOnFloor() && !Disparando)
		{

			if (Input.IsActionJustPressed("Jump"))
				velocity.Y = JumpVelocity;

			Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Back");
			Vector3 direction = (camera.GlobalBasis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				var temp = (camera.GlobalRotationDegrees.Y - GlobalRotationDegrees.Y);
				if (temp < -180)
					temp += 360;
				if (temp > 180)
					temp -= 360;

				var dir = temp * Vector3.Up * (float)delta * 10;
				GlobalRotationDegrees += dir;
				camera.GlobalRotationDegrees -= dir;
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			}
			if (Input.IsActionJustPressed("Fire2") && Torreta == null && area != 0)
			{
				var t = AutoLoad.auto.mundo.GetNode<LocalTorreta>(area + "/Torreta");
				if (t.torreta.IDPlayer == 0)
				{
					Torreta = t;
					t.torreta.IDPlayer = player.id;
                    GetNode<Camera>("Camera").GlobalRotation = Torreta.GlobalRotation;
					Disparando = true;
				}
			}
        }
		else if(IsOnFloor())
		{
            if (Input.IsActionJustPressed("Fire1"))
			{
				Torreta.Disparar();
			}
            else if (Input.IsActionJustPressed("Fire2") && Torreta != null)
            {
				Torreta.Base.Rotation = Vector3.Zero;
                Torreta.Pie.Rotation = Vector3.Zero;
                GetNode<Camera>("Camera").cam.RotationDegrees = Vector3.Zero;
                Disparando = false;
                Torreta.torreta.IDPlayer = 0;
                Torreta = null;
            }
        }
		if (AdminNet.admin.start) {
			if (timeSend > 0)
				timeSend -= delta * 4;
			else
			{
				timeSend = 1;
				player.pos = Position;
				player.vel = velocity;
				player.ry = GlobalRotationDegrees.Y;
				AdminNet.admin.cli.Send(player);
			}
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
