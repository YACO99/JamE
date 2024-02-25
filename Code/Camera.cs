using Godot;
using System;

public partial class Camera : Node3D
{
	Camera3D cam;
	RayCast3D rayRight, rayLeft;
	bool right = true;
	float sensX = 0.5f, sensY = 0.25f, speedCam = 10;

	public override void _Ready()
	{
		cam = GetNode<Camera3D>("Camera3D");
		rayRight = GetNode<RayCast3D>("RayRight");
		rayLeft = GetNode<RayCast3D>("RayLeft");
	}

	public override void _Process(double delta)
	{
		if (right)
		{
			if (rayRight.IsColliding())
				cam.GlobalPosition += (rayRight.GetCollisionPoint() - cam.GlobalPosition) * (float)delta * speedCam;
			else
				cam.Position += (new Vector3(1, 0, 3) - cam.Position) * (float)delta * speedCam;
		}
		else
		{
			if (rayLeft.IsColliding())
				cam.GlobalPosition += (rayLeft.GetCollisionPoint() - cam.GlobalPosition) * (float)delta * speedCam;
			else
				cam.Position += (new Vector3(-1, 0, 3) - cam.Position) * (float)delta * speedCam;
		}
	}

	public override void _Input(InputEvent e)
	{
		if (e is InputEventMouseMotion mm)
		{
			RotationDegrees += Vector3.Down * (mm.Relative.X * sensX);
			RotationDegrees += Vector3.Left * (mm.Relative.Y * sensY);
			RotationDegrees = new Vector3(Mathf.Clamp(RotationDegrees.X, -80, 80), RotationDegrees.Y, 0);
		}
	}
}
