#nullable enable

using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Resource
{

	public Vector2I TopLeftPosition{get;private set;}
	public Chunk?[,] MapChunks {get;private set;}

	private Queue<Chunk?> _chunkQueue;

	public Map(Vector2I TopLeftPosition) : base()
	{
		this.TopLeftPosition = TopLeftPosition;

		MapChunks = new Chunk?[Global.MAP_CHUNK_HEIGHT, Global.MAP_CHUNK_WIDTH];

		_chunkQueue = new Queue<Chunk?>();
		_chunkQueue.Enqueue(null);
		for (int yc = 0; yc < Global.MAP_CHUNK_HEIGHT; yc++)
		{
			for (int xc = 0; xc < Global.MAP_CHUNK_WIDTH; xc++)
			{
				MapChunks[yc, xc] = null;
			}
		}
	}

	private Chunk? QueueScrollOne()
	{
		Chunk? a = _chunkQueue.Dequeue();
		_chunkQueue.Enqueue(a);
		return a;
	}

	private void ScrollToFirst()
	{
		while (QueueScrollOne() is not null)
		{
			continue;
		}
	}

	private void EnqueueChunk(Chunk chunk)
	{
		while
		(
			(_chunkQueue.Peek() is not null) &&
			 (
				(_chunkQueue.Peek()!.TopLeftPosition.Y < chunk.TopLeftPosition.Y)
				||
				(_chunkQueue.Peek()!.TopLeftPosition.Y == chunk.TopLeftPosition.Y && _chunkQueue.Peek()!.TopLeftPosition.X < chunk.TopLeftPosition.X)
			 )
		)
		{
			QueueScrollOne();
		}
		if ((_chunkQueue.Peek() is null) || !(_chunkQueue.Peek()!.TopLeftPosition == chunk.TopLeftPosition))
		{
			_chunkQueue.Enqueue(chunk);
		}
		ScrollToFirst();
	}

	public Vector2I GetChunkCoordFromCoords(Vector2I Pos)
	{
		return new Vector2I(
			Mathf.FloorToInt((float)(Pos.X-TopLeftPosition.X) / Global.CHUNK_SIZE),
			Mathf.FloorToInt((float)(Pos.Y-TopLeftPosition.Y) / Global.CHUNK_SIZE));
	}

	public void LoadChunkEmptyBase(Vector2I ChunkCoord)
	{
		MapChunks[ChunkCoord.Y, ChunkCoord.X] = new Chunk(this,ChunkCoord * Global.CHUNK_SIZE + TopLeftPosition);
	}

	public void GenerateChunkBase(Vector2I ChunkCoord)
	{
		LoadChunkEmptyBase( ChunkCoord);
	}

	public bool IsChunkCoordInBounds(Vector2I ChunkCoord)
	{
		return !(ChunkCoord.X<0 || ChunkCoord.X>=Global.MAP_CHUNK_WIDTH || 
		ChunkCoord.Y<0 || ChunkCoord.Y>=Global.MAP_CHUNK_HEIGHT);
	}

	public bool IsPosReal(Vector2I Position)
	{
		Vector2I NullStartPos=Position-TopLeftPosition;
		return !(NullStartPos.X<0 || NullStartPos.X>=Global.MAP_CHUNK_WIDTH*Global.CHUNK_SIZE || 
		NullStartPos.Y<0 || NullStartPos.Y>=Global.MAP_CHUNK_HEIGHT*Global.CHUNK_SIZE);
	}

	public bool IsChunkLoadedBase(Vector2I ChunkCoord)
	{
		if (!IsChunkCoordInBounds( ChunkCoord))
		{return false;}
		return MapChunks[ChunkCoord.Y,ChunkCoord.X] is not null;
	}
	
	public Chunk? GetChunkAtPosForce(Vector2I Pos)
	{
		Vector2I ChunkCoord=GetChunkCoordFromCoords(Pos);
		if (!IsChunkCoordInBounds(ChunkCoord))
		{
			return null;
		}
		if (!IsChunkLoadedBase(ChunkCoord))
		{
			GenerateChunkBase(ChunkCoord);
		}
		return MapChunks[ChunkCoord.Y,ChunkCoord.X];
	}

	public Chunk? GetChunkAtPosIfLoaded(Vector2I Pos)
	{
		Vector2I ChunkCoord=GetChunkCoordFromCoords(Pos);
		if (!IsChunkCoordInBounds(ChunkCoord))
		{
			return null;
		}
		return MapChunks[ChunkCoord.Y,ChunkCoord.X];
	}

	public bool IsPosEmpty(Vector2I Pos)
	{
		Chunk? chHere= GetChunkAtPosForce(Pos);
		if (chHere is null)
		{
			return false;
		}
		return chHere.EntityAtGlobalPosition(Pos) is null;
	}

	public bool IsEntityOrNullAtPosForce(Entity? entity, Vector2I Pos)
	{
		if (!IsPosReal(Pos))
		{
			return false;
		}
		Entity? entMaybe = GetEntityAtPos(Pos);
		return entMaybe is null || entMaybe==entity;
	}

	public Entity? GetEntityAtPos(Vector2I globalPosition)
	{
		if (!IsPosReal(globalPosition))
		{
			return null;
		}
		Chunk? chHere= GetChunkAtPosForce(globalPosition);
		if (chHere is not null)
		{
			return chHere.EntityAtGlobalPosition(globalPosition);
		}
		return null;
	}
	
	public void SetPosTo(Entity? entity,Vector2I globalPosition)
	{
		if (!IsPosReal(globalPosition))
		{
			return;
		}
		Chunk? chHere= GetChunkAtPosForce(globalPosition);
		if (chHere is not null)
		{
			chHere.SetSlotGlobalForce(entity, globalPosition);
		}
	}

	public bool SetPosToSafe(Entity? entity,Vector2I globalPosition)
	{
		if (IsEntityOrNullAtPosForce(entity, globalPosition))
		{
			SetPosTo(entity,globalPosition);
			return true;
		}
		return false;
	}

	public void UpdateMap(int TickNumber)
	{
		if (TickNumber<0 || TickNumber>19)
		{
			return;
		}
		Chunk? scroller=QueueScrollOne();
		while (scroller is not null)
		{
			((Chunk)scroller).UpdateChunk(TickNumber);
			
			scroller=QueueScrollOne();
		}
	}
}
