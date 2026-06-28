#nullable enable
using Godot;
using System;
using System.Collections.Generic;
public partial class Map : Resource
{
	public Chunk?[,] MapChunks {get;private set;}
	private Queue<Chunk?> ChunkQueue;
	public Map()
	{
		MapChunks=new Chunk?[GlobalParameters.MAP_CHUNK_HEIGHT,GlobalParameters.MAP_CHUNK_WIDTH];
		ChunkQueue = new Queue<Chunk?>();
		ChunkQueue.Enqueue(null);
		for (int yc=0;yc<GlobalParameters.MAP_CHUNK_HEIGHT;yc++)
		{
			for (int xc=0;xc<GlobalParameters.MAP_CHUNK_WIDTH;xc++)
			{
				MapChunks[yc,xc]=null;
			}
		}
	}
	private Chunk? QueueScrollOne()
	{
		Chunk? a=ChunkQueue.Dequeue();
		ChunkQueue.Enqueue(a);
		return a;
	}
	private void ScrollToFirst()
	{
		while(QueueScrollOne() is not null)
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
				(ChunkQueue.Peek().TopLeftPos.Y<chunk.TopLeftPos.Y) 
				|| 
				(ChunkQueue.Peek().TopLeftPos.Y==chunk.TopLeftPos.Y && ChunkQueue.Peek().TopLeftPos.X<chunk.TopLeftPos.X)
			 )
		)
		{
			QueueScrollOne();
		}
		if ((ChunkQueue.Peek() is null) || !(ChunkQueue.Peek().TopLeftPos==chunk.TopLeftPos))
		{
			ChunkQueue.Enqueue(chunk);
		}
		ScrollToFirst();
	}
	
	public Vector2I GetChunkCoordFromCoords(Vector2I Pos)
	{
		return new Vector2I(
			Mathf.FloorToInt((float)Pos.X/GlobalParameters.CHUNK_SIZE),
			Mathf.FloorToInt((float)Pos.Y/GlobalParameters.CHUNK_SIZE));
	}
	public void LoadChunkEmptyBase(Vector2I ChunkCoord)
	{
		MapChunks[ChunkCoord.Y,ChunkCoord.X]=new Chunk(ChunkCoord*GlobalParameters.CHUNK_SIZE);
	}
	
}
