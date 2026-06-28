#nullable enable
using Godot;
using System;

using System.Collections.Generic;

public partial class Chunk : Resource
{
	public Vector2I TopLeftPos { get; private set; }
	
	public Chunk(Vector2I topLeftPos)
	{
		TopLeftPos = topLeftPos;
	}

	public bool IsSlotInMe(Vector2I Pos)
	{
		return !(Pos.X < TopLeftPos.X || Pos.X >= TopLeftPos.X + Global.CHUNK_SIZE
		|| Pos.Y < TopLeftPos.Y || Pos.Y >= TopLeftPos.Y + Global.CHUNK_SIZE
		);
	}
}
