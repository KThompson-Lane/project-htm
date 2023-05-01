using System;
using System.Collections.Generic;
using System.Linq;
using Code.DungeonGeneration;
using Code.Runtime.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    //  Room tiles for the tilemap
    public RuleTile WallTile;
    public DoorTile NorthDoor;
    public DoorTile EastDoor;
    public DoorTile SouthDoor;
    public DoorTile WestDoor;
    
    [SerializeField]
    protected TileList FloorTiles;

    public IEnumerable<Tuple<TileBase, Vector3Int>> GetFloorTiles()
    {
        return FloorTiles.Enumerate();
    }

    public void SetTilemaps(TileList floor)
    {
        FloorTiles = floor;
    }
    
    //  Room size
    public BoundsInt RoomBounds;
    
    //TODO: Is this needed?
    public RoomIndex Index;

    //  Is the room cleared
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
    public Dictionary<Direction, RoomIndex> Neighbours { get; set; } = new();
    public abstract void InitializeRoom();
    public void SetNeighbour(Direction direction, RoomIndex index)
    {
        Neighbours.Add(direction, index);
    }
    public RoomIndex? GetNeighbour(Direction direction)
    {
        return Neighbours.TryGetValue(direction, out var neighbour) ? neighbour : null;
    }
}
