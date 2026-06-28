#nullable enable
using Godot;
using System;

public partial class Chunk : Resource
{
	public const int CHUNK_SIZE=64;
	public Slot[,] Slots {get;set;} = new Slot[CHUNK_SIZE, CHUNK_SIZE];
	public Vector2I TopLeftPos;
	public Chunk(Vector2I TopLeftPos)
	{
		for (int y=0;y<CHUNK_SIZE;y++)
		{
			for (int x=0;x<CHUNK_SIZE;x++)
			{
				Slots[y,x]=new Slot(new Vector2I (TopLeftPos.X+x,TopLeftPos.Y+y));
			}
		}
	}
	public bool IsSlotInMe(Vector2I Pos)
	{
		return !(Pos.X<TopLeftPos.X || Pos.X>=TopLeftPos.X+CHUNK_SIZE
		|| Pos.Y<TopLeftPos.Y|| Pos.Y>=TopLeftPos.Y+CHUNK_SIZE
		);
	}
	public Slot? GetSlotGlobal(Vector2I Pos)
	{
		if (!IsSlotInMe(Pos))
		{
			return null;
		}
		return Slots[Pos.Y-TopLeftPos.Y,Pos.X-TopLeftPos.X];
	}
}
