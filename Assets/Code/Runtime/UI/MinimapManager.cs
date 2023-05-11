using System;
using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapManager : MonoBehaviour
{
    public DungeonFloor floorScript;
    [SerializeField] private Tilemap _dungeonFloorMap;
    [SerializeField] private Transform _playerMapIcon;
    [SerializeField] private MapTile bossRoomTile, startRoomTile, normalRoomTile;
    private Vector2Int _floorSize;
    private void Awake()
    {
        floorScript.OnRoomChange += DiscoverRoom;
        floorScript.OnRoomCleared += ClearRoom;
        floorScript.OnLevelChange += Reset;
    }

    public void Reset()
    {
        _dungeonFloorMap.ClearAllTiles();
        //  Place our start room onto the minimap
        var startRoom = floorScript.CurrentRoom;
        PlaceRoomTile(startRoom);
        //  Place the neighbours onto the minimap
        foreach (var (_, index) in startRoom.Neighbours)
        {
            PlaceRoomTile(floorScript.GetRoom(index));
        }
    }
    private void PlaceRoomTile(DungeonRoomScriptableObject room)
    {
        Vector3Int tilePosition = new Vector3Int(room.Index.X, room.Index.Y, 1);
        if (_dungeonFloorMap.HasTile(tilePosition))
            return;
        switch (room)
        {
            case BossRoomScriptableObject:
                _dungeonFloorMap.SetTile(tilePosition, Instantiate(bossRoomTile));
                break;
            case StartRoomScriptableObject:
                _dungeonFloorMap.SetTile(tilePosition, Instantiate(startRoomTile));
                _dungeonFloorMap.origin = tilePosition;
                _dungeonFloorMap.transform.localPosition = new Vector3(-room.Index.X, -room.Index.Y, 1);
                break;
            default:
                _dungeonFloorMap.SetTile(tilePosition, Instantiate(normalRoomTile));
                break;
        }
    }

    private void DiscoverRoom(RoomIndex room, Direction _)
    {
        //  Reveal our current room and place neighbour rooms

        Vector3Int tilePosition = new Vector3Int(room.X, room.Y, 1);
        _dungeonFloorMap.GetTile<MapTile>(tilePosition).Discover();
        _dungeonFloorMap.RefreshTile(tilePosition);
        foreach (var (_, index) in floorScript.GetRoom(room).Neighbours)
        {
            PlaceRoomTile(floorScript.GetRoom(index));
        }
        //  Also move player
        _dungeonFloorMap.transform.localPosition = new Vector3(-room.X, -room.Y, 1);
        _playerMapIcon.position = _dungeonFloorMap.CellToWorld(tilePosition);
    }

    private void ClearRoom(RoomIndex room)
    {
        Vector3Int tilePosition = new Vector3Int(room.X, room.Y, 1);
        _dungeonFloorMap.GetTile<MapTile>(tilePosition).Clear();
        _dungeonFloorMap.RefreshTile(tilePosition);
    }
}
