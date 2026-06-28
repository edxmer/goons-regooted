using Godot;
using System;

public partial class Entity : Resource
{
	public int Weight{get;protected set;}
	public Vector2I TopLeftPosition{get; set;}
	/*Global pos, based on the map*/
	public bool[,] SizeMap {get;protected set;}
	
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
	public Entity(Map map,Vector2I TopLeftPos)
	{
		this.myMap=map;
		this.TopLeftPosition=TopLeftPos;
		this.SizeMap=new bool[,] {{true}};
		this.Weight=1;
	}
}
