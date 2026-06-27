using Godot;
using System;

public partial class Slot : Resource
{
	public Entity? EntityHere {get;set;};
	public Vector2I Position {get; private set}
	public Slot(Vector2I Position)
	{
		this.Position=Position;
		this.EntityHere=null;
	}
}
