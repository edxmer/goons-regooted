#nullable enable
using Godot;
using System;

public partial class Chunk : Resource
{
	public Vector2I TopLeftPos { get; private set; }

	public readonly Entity[,] EntityGrid = new Entity[Global.CHUNK_SIZE, Global.CHUNK_SIZE];
	
	public Chunk(Vector2I topLeftPos)
	{
		TopLeftPos = topLeftPos;
	}

	public bool DoesContainPosition(Vector2I position)
	{
		return !(position.X < TopLeftPos.X || position.X >= TopLeftPos.X + Global.CHUNK_SIZE
		|| position.Y < TopLeftPos.Y || position.Y >= TopLeftPos.Y + Global.CHUNK_SIZE
		);
	}

	public bool HasEntityAtLocalPosition(Vector2I position)
	{
		if (Global.CHUNK_SIZE <= position.X || Global.CHUNK_SIZE <= position.Y) return false;

		return EntityGrid[position.X, position.Y] is not null;
	}


}
