using System;
using Godot;

public static class Global
{
	/* --- MAP CONSTANTS --- */
	public const int CHUNK_SIZE = 64;
	public const int MAP_CHUNK_WIDTH = 100;
	public const int MAP_CHUNK_HEIGHT = 100;
	
	/* --- INSTANCES --- */
	public static Map Map { get; set; }
	public static PlayerCamera PlayerCamera { get; set; }
}
