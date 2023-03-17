using Unity.VisualScripting;
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

    //  Add neighbouring cells (0 means does not exist)
    [DoNotSerialize] public int northNeighbour;
    [DoNotSerialize] public int eastNeighbour;
    [DoNotSerialize] public int southNeighbour;
    [DoNotSerialize] public int westNeighbour;
}
