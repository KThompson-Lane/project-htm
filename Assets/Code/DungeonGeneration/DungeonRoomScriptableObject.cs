using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    public RuleTile FloorTile;
    public RuleTile WallTile;
    public BoundsInt RoomBounds;

    public bool Cleared;
    //TODO:
    //  Add positions of obstacles

    //  Add neighbouring room options
    [DoNotSerialize][CanBeNull] public DungeonRoomScriptableObject northNeighbour;
    [DoNotSerialize][CanBeNull] public DungeonRoomScriptableObject eastNeighbour;
    [DoNotSerialize][CanBeNull] public DungeonRoomScriptableObject southNeighbour;
    [DoNotSerialize][CanBeNull] public DungeonRoomScriptableObject westNeighbour;
}
