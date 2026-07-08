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
			new TestStaticEntity(Vector2I.Zero) {Texture = GD.Load<Texture2D>("res://assets/sprites/items/rock/spr_item_rock.png")},
			Vector2I.Zero
		);
	}

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
				Entity entity = Global.Map.GetEntityAtPos(new Vector2I(x, y));

				GD.PrintRaw($"Drawing entity at position ({x}, {y})... ");

				if (entity is not null && entity.Texture is not null)
				{
					var rect = new Rect2(
						new Vector2(x*PixelsPerTile, y*PixelsPerTile), 
						new Vector2((x+1)*PixelsPerTile, (y+1)*PixelsPerTile)
					);

					DrawTextureRect(entity.Texture, rect, tile: false);

					GD.PrintRaw("Success drawing texture.");
				}

				GD.Print("");
			}
    }
}
