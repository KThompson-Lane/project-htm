using System;
using System.Linq;
using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class DungeonFloor : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    public DungeonFloorScriptableObject floorObject;
    public Tilemap floorPreview;
    public TileBase normalRoomTile;
    public TileBase startRoomTile;
    public TileBase bossRoomTile;
    
    private Tilemap _floorMap, _wallsMap, _doorMap;
    private RoomDoors _roomDoors;
    
    private void Start()
    {
        //  First generate dungeon floor
        GenerateFloor();
        Debug.Log("Floor generated Successfully");

        //  Then load start room
        int startCell = ((floorObject.floorSize.y / 2) * 10) + (floorObject.floorSize.x / 2);
        var startRoom = floorObject.floorplan[startCell]; 
        Debug.Log("Loading start room");
        LoadRoom(startRoom);
        _roomDoors.OnRoomChange += OnRoomChange;
    }

    public void GenerateFloor()
    {
        if (floorObject != null)
            floorObject.GenerateFloor();
        if (floorPreview != null)
        {
            var floorSize = floorObject.floorSize;
            floorPreview.size = new Vector3Int(floorSize.x, floorSize.y, 1);
            floorPreview.origin = new Vector3Int(0, 0, 0);
            floorPreview.ResizeBounds();
            foreach (var room in floorObject.floorplan)
            {
                //  Calculate tile position
                int roomPosition = room.Key;
                int roomX = roomPosition % 10;
                int roomY = roomPosition / 10;
                Vector3Int tilePosition = new Vector3Int(roomX - (floorSize.x / 2), -roomY - floorSize.y / 2, 1);
                //  Starter room
                if (room.Value != null)
                {
                    switch (room.Value)
                    {
                        case BossRoomScriptableObject:
                            floorPreview.SetTile(tilePosition, bossRoomTile);
                            break;
                        case StartRoomScriptableObject:
                            floorPreview.SetTile(tilePosition, startRoomTile);
                            break;
                        default:
                            floorPreview.SetTile(tilePosition, normalRoomTile);
                            break;
                    }
                }
            }
            floorPreview.CompressBounds();
        }
    }
    
    public void LoadRoom(DungeonRoomScriptableObject roomData)
    {
        //  Move player to centre
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
        _doorMap = mTilemaps.First(map => map.name == "Doors");
        _roomDoors = gameObject.GetComponentInChildren<RoomDoors>();
        player.transform.position = Vector3.zero;
        if (roomData != null)
        {
            _floorMap.ClearAllTiles();
            _wallsMap.ClearAllTiles();
            _doorMap.ClearAllTiles();
            
            _floorMap.origin = roomData.RoomBounds.position;
            _floorMap.size = roomData.RoomBounds.size;
            _floorMap.ResizeBounds();
            _floorMap.FloodFill(Vector3Int.zero, roomData.FloorTile);
            
            _wallsMap.origin = roomData.RoomBounds.position;
            _wallsMap.size = roomData.RoomBounds.size;
            _wallsMap.ResizeBounds();
            _wallsMap.FloodFill(Vector3Int.zero, roomData.WallTile);
            
            _doorMap.origin = roomData.RoomBounds.position;
            _doorMap.size = roomData.RoomBounds.size;
            _doorMap.ResizeBounds();
            
            //  Fill in doors
            if (roomData.northNeighbour != 0)
            {
                var position = new Vector3Int(0, roomData.RoomBounds.size.y / 2, 0); 
                var neighbourRoom = roomData.northNeighbour;
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }

            if (roomData.southNeighbour != 0)
            {
                var position = new Vector3Int(0, -roomData.RoomBounds.size.y / 2, 0); 
                var neighbourRoom = roomData.southNeighbour;
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }
            
            if (roomData.eastNeighbour != 0)
            {
                var position = new Vector3Int(roomData.RoomBounds.size.x / 2, 0, 0); 
                var neighbourRoom = roomData.eastNeighbour;
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }
            
            if (roomData.westNeighbour != 0)
            {
                var position = new Vector3Int(-roomData.RoomBounds.size.x / 2, 0, 0); 
                var neighbourRoom = roomData.westNeighbour;
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }
        }
    }
    private void OnRoomChange(int roomIndex)
    {
        var newRoom = floorObject.floorplan[roomIndex];
        Debug.Log($"Loading room {roomIndex}");
        LoadRoom(newRoom);
    }

}
