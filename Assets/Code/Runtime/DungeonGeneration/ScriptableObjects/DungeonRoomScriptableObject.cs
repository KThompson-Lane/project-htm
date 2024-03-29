using System.Collections.Generic;
using Code.DungeonGeneration;
using UnityEngine;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    public RoomLayout layout;
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
    public static event ClearRoom OnRoomCleared;
    //TODO:
    //  Add positions of obstacles
    public Dictionary<Direction, RoomIndex> Neighbours { get; set; } = new();
    public abstract void InitializeRoom(int level = 1);
    public void SetNeighbour(Direction direction, RoomIndex index)
    {
        Neighbours.Add(direction, index);
    }
    public RoomIndex? GetNeighbour(Direction direction)
    {
        return Neighbours.TryGetValue(direction, out var neighbour) ? neighbour : null;
    }
}
