#nullable enable

using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Entity : Resource
{
	public int Weight { get; protected set; }
	public bool IsOnMap {get;protected set;}
	public int LastUpdatedTick;
	public Vector2I TopLeftPosition { get; set; }
	/*Global pos, based on the map*/
	public Vector2I LocalChunkTopLeftPosition /* Position within chunk */
	{
		get
		{
			return TopLeftPosition%Global.CHUNK_SIZE;
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
		LastUpdatedTick=-1;
		this.myMap = map;
		this.TopLeftPosition = TopLeftPos;
		this.SizeMap = new bool[,] { { true } };
		this.Weight = 1;
		IsOnMap=false;
	}
	
	public Chunk? GetMyMainChunk()
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
		return TopLeftPosition+localPose;
	}
	public List<Vector2I> GetAllContainedPositions()
	{
		List<Vector2I> poses=GetAllLocalPositions();
		
		for (var i=0;i< poses.Count;i++)
		{
			poses[i]=EnLocalPositionToGlobal(poses[i]);

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
			if (!(ch is null) && !chunks.Contains(ch))
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
		GetMyMainChunk()?.RemoveEntityFromUpdater(this);
	}
	
	public bool CouldAddToMapAtPos(Vector2I TopLeftPosition)
	{
		List<Vector2I> poses=GetAllLocalPositions();
		
		int ind=0;
		while (ind<poses.Count && myMap.IsEntityOrNullAtPosForce(this, TopLeftPosition+poses[ind]))
		{ind++;}
		return (ind>=poses.Count);
		
	}
	protected bool AddToMapForce(Vector2I TopLeftPosition)
	{
		this.TopLeftPosition=TopLeftPosition;
		
		List<Vector2I> poses=GetAllLocalPositions();
		for (int i=0; i<poses.Count ;i++)
		{
			poses[i]=TopLeftPosition+poses[i];
		}
		if (IsOnMap)
		{
			RemoveFromMap();
		}
		IsOnMap=true;
		foreach (Vector2I globalPos in poses)
		{
			bool Success = myMap.SetPosToSafe(this,globalPos);
			if (!Success)
			{
				RemoveFromMap();
				return false;
			}
			
		}
		GetMyMainChunk()!.LogEntityToUpdater(this);
		return true;
	}
	public bool TryAddToMapAt(Vector2I TopLeftPosition)
	{
		if (CouldAddToMapAtPos( TopLeftPosition))
		{
			return AddToMapForce(TopLeftPosition);
			
		}
		return false;
	}
	
	public bool TryMoveTo(Vector2I NewTopLeftPosition)
	{
		Vector2I OldPosition=TopLeftPosition;
		bool WasOnMap=IsOnMap;
		if(TryAddToMapAt(NewTopLeftPosition))
		{
			return true;
		}
		if (WasOnMap)
		{
			TryAddToMapAt( OldPosition);
		}
		return false;
	}
}
