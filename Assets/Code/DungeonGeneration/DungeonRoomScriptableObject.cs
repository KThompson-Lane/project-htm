using UnityEngine;

/// <summary>
/// An abstract scriptable object class to represent a dungeon room
/// </summary>
public abstract class DungeonRoomScriptableObject : ScriptableObject
{
    public RuleTile FloorTile;
    public RuleTile WallTile;
    public BoundsInt RoomBounds;
    //TODO:
    //  Add positions of obstacles
    //  Add neighbouring room options
}
