#nullable enable
using Godot;
using System;
using System.Collections.Generic;
public partial class Map : Resource
{

	public Vector2I TopLeftPosition{get;private set;}
	public Chunk?[,] MapChunks {get;private set;}

	private Queue<Chunk?> ChunkQueue;
	public Map(Vector2I TopLeftPosition)
	{

		this.TopLeftPosition = TopLeftPosition;

		MapChunks = new Chunk?[Global.MAP_CHUNK_HEIGHT, Global.MAP_CHUNK_WIDTH];

		ChunkQueue = new Queue<Chunk?>();
		ChunkQueue.Enqueue(null);
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
		Chunk? a = ChunkQueue.Dequeue();
		ChunkQueue.Enqueue(a);
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
			(ChunkQueue.Peek() is not null) &&
			 (
				(ChunkQueue.Peek().TopLeftPosition.Y < chunk.TopLeftPosition.Y)
				||
				(ChunkQueue.Peek().TopLeftPosition.Y == chunk.TopLeftPosition.Y && ChunkQueue.Peek().TopLeftPosition.X < chunk.TopLeftPosition.X)
			 )
		)
		{
			QueueScrollOne();
		}
		if ((ChunkQueue.Peek() is null) || !(ChunkQueue.Peek().TopLeftPosition == chunk.TopLeftPosition))
		{
			ChunkQueue.Enqueue(chunk);
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
	public bool IsEntityOrNullAtPosForce(Entity entity, Vector2I Pos)
	{
		if (!IsPosReal(Pos))
		{
			return false;
		}
		Entity? entMaybe=GetEntityAtPos(Pos);
		return (entMaybe is null || entMaybe==entity);
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
		if (IsEntityOrNullAtPosForce( entity, globalPosition))
		{
			SetPosTo(entity,globalPosition);
			return true;
		}
		return false;
	}
	
	
}
