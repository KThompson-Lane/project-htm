using System;
using System.Collections.Generic;
using System.Linq;
using Code.DungeonGeneration;
using Code.Runtime.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLayout : ScriptableObject
{
    //  Room tiles for the tilemap
    public RuleTile WallTile;
    public DoorTile NorthDoor;
    public DoorTile EastDoor;
    public DoorTile SouthDoor;
    public DoorTile WestDoor;
    
    [SerializeField]
    public TileList FloorTiles;

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

    [NonSerialized] public List<Vector3Int> doorPositions;

    public float DistanceToClosestDoor(Vector3Int position)
    {
        var closest = doorPositions.OrderBy(door => (door - position).sqrMagnitude).First();
        return Vector3Int.Distance(closest, position);
    }
}
