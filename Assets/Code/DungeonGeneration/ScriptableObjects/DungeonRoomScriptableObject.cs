using System.Collections.Generic;
using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    public RuleTile FloorTile;
    public RuleTile WallTile;
    public TileBase DoorTile;
    public BoundsInt RoomBounds;
    
    public bool Cleared;
    //TODO:
    //  Add positions of obstacles
    public Dictionary<Direction, int> Neighbours { get; set; } = new();
    public void SetNeighbour(Direction direction, int room)
    {
        Neighbours.Add(direction, room);
    }
    public int GetNeighbour(Direction direction)
    {
        return Neighbours.ContainsKey(direction) ? Neighbours[direction] : 0;
    }
}
