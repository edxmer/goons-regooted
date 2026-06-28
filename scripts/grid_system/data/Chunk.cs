using Godot;
using System;

public partial class Chunk : Resource
{
	public static int CHUNKSIZE=64;
	public Slot[,] Slots {get;set;}
	public Vector2I TopLeftPos;
	public Chunk(Vector2I TopLeftPos)
	{
		Slots=new Slot[CHUNKSIZE,CHUNKSIZE];
		for (int y=0;y<CHUNKSIZE;y++)
		{
			for (int x=0;x<CHUNKSIZE;x++)
			{
				Slots[y,x]=new Slot(new Vector2I (TopLeftPos.X+x,TopLeftPos.Y+y));
			}
		}
	}
	public Slot? GetSlotGlobal(Vector2I Pos)
	{
		if (Pos.X<TopLeftPos.X || Pos.X>=TopLeftPos.X+CHUNKSIZE
		|| Pos.Y<TopLeftPos.Y|| Pos.Y>=TopLeftPos.Y+CHUNKSIZE
		)
		{
			return null;
		}
		return Slots[Pos.Y-TopLeftPos.Y,Pos.X-TopLeftPos.X];
	}
}
