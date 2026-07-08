using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	[Export] public float ZoomSpeed = 0.1f;

	public override void _Ready()
	{
		Global.PlayerCamera = this;
	}

	public override void _Process(double delta)
	{
		float deltaZoom = 0;
		if (Input.IsActionJustPressed("camera_zoom_in"))
		{
			deltaZoom = 1;
		}
		else if (Input.IsActionJustPressed("camera_zoom_out"))
		{
			deltaZoom = -1;

		}
		deltaZoom *= ZoomSpeed;
		Zoom = new Vector2(Zoom.X + deltaZoom, Zoom.Y + deltaZoom);
		if (Zoom.X < 1)
		{
			Zoom = new Vector2(1, 1);
		}
	}

	public Rect2 GetCameraViewRect()
	{
		Vector2 screenCenter = GetScreenCenterPosition();
		Vector2 halfViewSize = GetViewportRect().Size / 2 / Zoom;

		return new Rect2(screenCenter - halfViewSize, halfViewSize * 2);
	}
}
