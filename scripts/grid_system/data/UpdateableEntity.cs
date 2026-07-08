using Godot;
using System;

public abstract partial class UpdateableEntity : Entity
{
	public virtual bool HasUpdateFunction => true;

	public UpdateableEntity(Vector2I TopLeftPos): base(TopLeftPos) {}
	/*TickNumber is a number between 0-19*/
	public abstract void OnUpdate(int TickNumber);
}
