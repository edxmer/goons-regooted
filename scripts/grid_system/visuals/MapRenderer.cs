using Godot;
using System;

[GlobalClass]
public partial class MapRenderer : Node2D
{
	[Export]
	public float PixelsPerTile { get; set; } = 16;

	public override void _Ready()
	{
		// TODO: this is probably not the best place to create the map, 
		// but it will suffice for testing purposes
		Global.Map = new Map();

		// TODO: Spawn in test rock at (0, 0), should remove later
		Global.Map.SetPosTo(
			new TestStaticEntity(Vector2I.Zero) { Texture = GD.Load<Texture2D>("res://assets/sprites/items/rock/spr_item_rock.png") },
			Vector2I.Zero
		);

		Global.Map.SetPosTo(
			new TestStaticEntity(new Vector2I(1, 1)) { Texture = GD.Load<Texture2D>("res://assets/sprites/items/rock/spr_item_rock.png") },
			new Vector2I(1, 1)
		);

		Global.Map.SetPosTo(
			new TestStaticEntity(new Vector2I(2, 1)) { 
				Texture = GD.Load<Texture2D>("res://assets/sprites/items/rock/spr_item_rock.png"),
				Shape = new bool[,] { {true, true} }
				},
			new Vector2I(2, 1)
		);
	}

	// TODO: We need to create a signal that will call QueueRedraw() when something updates
	// TODO: We should also add animations when tiles move, but only after 
	// the basic functionalities are complete.

	public override void _Draw()
	{
		base._Draw();

		if (Global.Map is null || Global.PlayerCamera is null) return;

		Rect2 cameraViewRect = Global.PlayerCamera.GetCameraViewRect();
		Vector2 cameraTopLeftCorner = cameraViewRect.Position;
		Vector2 cameraBotRightCorner = cameraTopLeftCorner + cameraViewRect.Size;

		Vector2I topLeftCoords = new(
			Mathf.CeilToInt(cameraTopLeftCorner.X / (float)PixelsPerTile),
			Mathf.CeilToInt(cameraTopLeftCorner.Y / (float)PixelsPerTile)
		);

		Vector2I botRightCoords = new(
			Mathf.FloorToInt(cameraBotRightCorner.X / (float)PixelsPerTile),
			Mathf.FloorToInt(cameraBotRightCorner.Y / (float)PixelsPerTile)
		);

		for (int x = topLeftCoords.X; x <= botRightCoords.X; ++x)
			for (int y = topLeftCoords.Y; y <= botRightCoords.Y; ++y)
			{
				// TODO: Draw Biome

				

				// Draw entity

				Entity entity = Global.Map.GetEntityAtPos(new Vector2I(x, y));

				var coords = new Vector2I(x, y);

				if (entity is not null && entity.Texture is not null && entity.TopLeftPosition == coords)
				{
					Vector2 rectTopLeftCorner = new Vector2(x * PixelsPerTile, y * PixelsPerTile);
					Vector2 rectSize = new Vector2(entity.GetWidth() * PixelsPerTile, entity.GetHeight() * PixelsPerTile);

					var rect = new Rect2(
						rectTopLeftCorner,
						rectSize
					);

					DrawTextureRect(entity.Texture, rect, tile: false);
				}
			}
	}
}
