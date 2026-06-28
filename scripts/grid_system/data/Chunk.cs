#nullable enable
using Godot;
using System;

using System.Collections.Generic;

public partial class Chunk : Resource
{
	public Slot[,] Slots { get; set; } = new Slot[GlobalParameters.CHUNK_SIZE, GlobalParameters.CHUNK_SIZE];
	public Vector2I TopLeftPos;
	public Chunk(Vector2I TopLeftPos)
	{
		for (int y = 0; y < GlobalParameters.CHUNK_SIZE; y++)
		{
			for (int x = 0; x < GlobalParameters.CHUNK_SIZE; x++)
			{
				Slots[y, x] = new Slot(new Vector2I(TopLeftPos.X + x, TopLeftPos.Y + y));
			}
		}
	}
	public bool IsSlotInMe(Vector2I Pos)
	{
		return !(Pos.X < TopLeftPos.X || Pos.X >= TopLeftPos.X + GlobalParameters.CHUNK_SIZE
		|| Pos.Y < TopLeftPos.Y || Pos.Y >= TopLeftPos.Y + GlobalParameters.CHUNK_SIZE
		);
	}
	public Slot? GetSlotGlobal(Vector2I Pos)
	{
		if (!IsSlotInMe(Pos))
		{
			return null;
		}
		return Slots[Pos.Y - TopLeftPos.Y, Pos.X - TopLeftPos.X];
	}
}
