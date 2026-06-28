using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Entity : Resource
{
	public int Weight { get; protected set; }
	public Vector2I TopLeftPosition { get; set; }
	/*Global pos, based on the map*/
	public Vector2I LocalTopLeftPosition /* Position within chunk */
	{
		get
		{
			return myMap.GetChunkCoordFromCoords(TopLeftPosition);
		}
	}
	public bool[,] SizeMap { get; protected set; }

	private Map myMap;

	/*true is something there, [y,x]*/
	public int GetWidth()
	{
		return SizeMap.GetLength(1);
	}
	public int GetHeight()
	{
		return SizeMap.GetLength(0);
	}
	public Entity(Map map, Vector2I TopLeftPos)
	{
		this.myMap = map;
		this.TopLeftPosition = TopLeftPos;
		this.SizeMap = new bool[,] { { true } };
		this.Weight = 1;
	}
	
	public Chunk GetMyMainChunk()
	{
		return myMap.GetChunkAtPosForce(TopLeftPosition);
	}
	public List<Vector2I> GetAllContainedPositions()
	{
		List<Vector2I> poses=new();
		for(int y=0;y<GetHeight();y++)
		{
			for(int x=0;x<GetWidth();x++)
			{
				if (SizeMap[y,x])
				{
					poses.Add(new Vector2I(TopLeftPosition.X+x,TopLeftPosition.Y+y));
				}
			}
		}
		return poses;
	}
	
	
	public List<Chunk> GetAllContainedChunks()
	{
		List<Chunk> chunks=new();
		List<Vector2I> poses=GetAllContainedPositions();
		foreach (Vector2I pos in poses)
		{
			if (SizeMap[pos.Y,pos.X])
			{
				var ch=myMap.GetChunkAtPosForce(pos);
				if (!chunks.Contains(ch))
				{
					chunks.Add(ch);
				}
			}
			
		}
		return chunks;
	}
}
