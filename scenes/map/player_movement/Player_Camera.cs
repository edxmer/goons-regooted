using Godot;
using System;

public partial class Player_Camera : Camera2D
{	
	[Export]public float ZoomSpeed=0.1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float deltaZoom=0;
		if (Input.IsActionJustPressed("camera_zoom_in"))
		{
			deltaZoom=1;
		}
		else if(Input.IsActionJustPressed("camera_zoom_out"))
		{
			deltaZoom=-1;
			
		}
		deltaZoom*=ZoomSpeed;
		Zoom =new Vector2(Zoom.X+deltaZoom,Zoom.Y+deltaZoom);
		if (Zoom.X<1)
		{
			Zoom=new Vector2(1,1);
		}
	}
}
