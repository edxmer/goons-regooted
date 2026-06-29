#nullable enable

using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Entity : Resource
{
	public int Weight { get; protected set; }
	public bool IsOnMap {get;protected set;}
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
		IsOnMap=false;
	}
	
	public Chunk GetMyMainChunk()
	{
		return myMap.GetChunkAtPosForce(TopLeftPosition);
	}
	
	public List<Vector2I> GetAllLocalPositions()
	{
		List<Vector2I> poses=new();
		for(int y=0;y<GetHeight();y++)
		{
			for(int x=0;x<GetWidth();x++)
			{
				if (SizeMap[y,x])
				{
					poses.Add(new Vector2I(x,y));
				}
			}
		}
		return poses;
	}
	public Vector2I EnLocalPositionToGlobal(Vector2I localPose)
	{
		return new Vector2I(TopLeftPosition.X+localPose.X,TopLeftPosition.Y+localPose.Y);
	}
	public List<Vector2I> GetAllContainedPositions()
	{
		List<Vector2I> poses=new();
		List<Vector2I> localPoses=GetAllLocalPositions();
		
		foreach (Vector2I local in localPoses)
		{
			poses.Add(EnLocalPositionToGlobal(local));

		}
		
		return poses;
	}
	
	public List<Chunk> GetAllContainedChunks()
	{
		List<Chunk> chunks=new();
		List<Vector2I> localPoses=GetAllLocalPositions();
		foreach (Vector2I local in localPoses)
		{
			var ch=myMap.GetChunkAtPosForce(EnLocalPositionToGlobal(local));
			if (!chunks.Contains(ch))
			{
				chunks.Add(ch);
			}
			
		}
		return chunks;
	}
	
	
	public void RemoveFromMap()
	{
		if (!IsOnMap)
		{
			return;
		}
		List<Vector2I> poses=GetAllContainedPositions();
		foreach (Vector2I pos in poses)
		{
			Chunk? chCur=myMap.GetChunkAtPosIfLoaded(pos);
			if (chCur is not null)
			{
				chCur.RemoveEntitySlotFromGlobalPos(this,pos);
			}
		}
		
		IsOnMap=false;
	}
}
