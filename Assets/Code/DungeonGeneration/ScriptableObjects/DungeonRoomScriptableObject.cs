using System.Collections.Generic;
using Code.DungeonGeneration;
using UnityEngine;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    public RuleTile FloorTile;
    public RuleTile WallTile;
    public DoorTile NorthDoor;
    public DoorTile EastDoor;
    public DoorTile SouthDoor;
    public DoorTile WestDoor;
    
    public BoundsInt RoomBounds;
    
    private bool _cleared;
    public bool Cleared
    {
        get => _cleared;
        set
        {
            _cleared = value;
            OnRoomCleared?.Invoke();
        }
    }

    public delegate void ClearRoom();
    public event ClearRoom OnRoomCleared;
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
