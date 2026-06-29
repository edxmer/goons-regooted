#nullable enable
using Godot;
using System;
using System.Collections.Generic;

public partial class Chunk : Resource
{
	public Vector2I TopLeftPos { get; private set; }

	public readonly Entity?[,] EntityGrid = new Entity?[Global.CHUNK_SIZE, Global.CHUNK_SIZE];
	
	public LinkedList<Entity> Entities;
	
	public Chunk(Vector2I topLeftPos)
	{
		TopLeftPos = topLeftPos;
		Entities=new();
	}
	
	protected Vector2I LocalPositionToGlobal(Vector2I localPosition)
	{
		return localPosition+TopLeftPos;
	}
	protected Vector2I GlobalPositionToLocal(Vector2I globalPosition)
	{
		return globalPosition-TopLeftPos;
	}
	
	
	public bool DoesContainLocalPosition(Vector2I localPosition)
	{
		return !(localPosition.X < 0 || localPosition.X >= Global.CHUNK_SIZE
		|| localPosition.Y < 0 || localPosition.Y >= Global.CHUNK_SIZE
		);
	}
	public bool DoesContainGlobalPosition(Vector2I position)
	{
		return DoesContainLocalPosition(position-TopLeftPos);
	}

	public bool HasEntityAtLocalPosition(Vector2I localPosition)
	{
		return EntityAtLocalPosition(localPosition) is not null;
	}
	public Entity? EntityAtLocalPosition(Vector2I localPosition)
	{
		if (DoesContainLocalPosition(localPosition))
		{
			return EntityGrid[localPosition.Y, localPosition.X];
		}
		return null;
	}
	public Entity? EntityAtGlobalPosition(Vector2I globalPosition)
	{
		Vector2I localPosition=GlobalPositionToLocal(globalPosition);
		if (DoesContainLocalPosition(localPosition))
		{
			return EntityGrid[localPosition.Y, localPosition.X];
		}
		return null;
	}
	
	private bool ResetSlotLocal(Vector2I localPosition)
	{
		if (HasEntityAtLocalPosition(localPosition))
		{
			EntityGrid[localPosition.Y, localPosition.X] = null;
			return true;
		}
		return false;
	}

	
	public bool ResetSlotGlobal(Vector2I globalPosition)
	{
		return ResetSlotLocal(GlobalPositionToLocal(globalPosition));
	}
	public bool RemoveEntitySlotFromLocalPos(Entity entity,Vector2I localPosition)
	{
		if (EntityAtLocalPosition(localPosition)==entity)
		{
			return ResetSlotLocal(localPosition);
		}
		return false;
	}
	public bool RemoveEntitySlotFromGlobalPos(Entity entity,Vector2I globalPosition)
	{
		return RemoveEntitySlotFromLocalPos(entity,GlobalPositionToLocal(globalPosition));
	}
	public bool IsEntityInUpdater(Entity entity)
	{
		return Entities.Contains(entity);
	}
	public void RemoveEntityFromUpdater(entity)
	{
		if (IsEntityInUpdater(entity))
		{
			Entities.Remove(entity);
		}
	}
	
	public bool LogEntityToUpdater(Entity entity)
	{
		if (IsEntityInUpdater(entity))
		{
			
		}
		if (entity is UpdateableEntity upd)
		{
			if (upd.HasUpdateFunction)
			{
				int ind=0;
				while(ind<Entities.Size && (Entities[ind].TopLeftPosition.Y<entity.TopLeftPosition.Y || (Entities[ind].TopLeftPosition.Y==entity.TopLeftPosition.Y && Entities[ind].TopLeftPosition.X<entity.TopLeftPosition.X )))
				{
					ind++;
				}
				
			}
		}
	}
}
