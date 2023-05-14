using System;
using System.Collections.Generic;
using System.Linq;
using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Code.Runtime.DungeonGeneration
{
    [RequireComponent(typeof(Grid))]
    public class RoomCreator : MonoBehaviour
    {
        //  Child tilemaps
        public Tilemap Floor;
        public TileList FloorTiles { get; set; } = new();
        public BoundsInt RoomBounds;
        //  Room tiles for the tilemap
        public RuleTile WallTile;
        public DoorTile NorthDoor;
        public DoorTile EastDoor;
        public DoorTile SouthDoor;
        public DoorTile WestDoor;
        public RoomLayout layoutToLoad;
        public void LoadLayout()
        {
            foreach (var (tile, position) in layoutToLoad.GetFloorTiles())
            {
                Floor.SetTile(position, tile);
            }
        }

        public void SaveLayout()
        {
            FloorTiles = new();
            Floor.CompressBounds();
            RoomBounds = Floor.cellBounds;
            foreach (var position in RoomBounds.allPositionsWithin)
            {
                var tile = Floor.GetTile(position);
                if(tile == null)
                    continue;
                if (!FloorTiles.TryAdd(new TileSet(tile, new List<Vector3Int>{position})))
                    FloorTiles[tile].Positions.Add(position);
            }
            Debug.Log("loaded all tiles");
        }

        public void CreateRoom()
        {
            FloorTiles = new();
            Floor.CompressBounds();
            RoomBounds = Floor.cellBounds;
            foreach (var position in RoomBounds.allPositionsWithin)
            {
                var tile = Floor.GetTile(position);
                if(tile == null)
                    continue;
                if (!FloorTiles.TryAdd(new TileSet(tile, new List<Vector3Int>{position})))
                    FloorTiles[tile].Positions.Add(position);
            }
            Debug.Log("loaded all tiles");
        }
        
    }
}