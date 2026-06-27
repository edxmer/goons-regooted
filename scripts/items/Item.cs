using Godot;
using System;

public partial class Item : Area2D
{
	private AnimatedSprite2D mySprite;
	private CollisionShape2D myBoundary;
	private ItemData myData;
	private string myName;
	public void Assign (ItemData myData)
	{
		this.myData=myData;
		mySprite.SpriteFrames=myData.itemSprite;
		mySprite.Play("default");
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mySprite=GetNode<AnimatedSprite2D>("Sprite");
		myBoundary=GetNode<CollisionShape2D>("ItemBoundary");
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
