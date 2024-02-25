using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	Camera camera;
	Vector3 velocity;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

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


		if (IsOnFloor())
		{
			
			if (Input.IsActionJustPressed("Jump"))
				velocity.Y = JumpVelocity;
				
			Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Back");
			Vector3 direction = (camera.GlobalBasis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				var temp=(camera.GlobalRotationDegrees.Y - GlobalRotationDegrees.Y);
				if (temp < -180)
					temp += 360;
                if (temp > 180)
                    temp -= 360;

                var dir = temp  * Vector3.Up * (float)delta*10;
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
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
