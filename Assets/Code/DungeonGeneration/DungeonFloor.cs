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

    private void Awake()
    {
        //  Get tilemaps and room doors script
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
        _doorMap = mTilemaps.First(map => map.name == "Doors");
        _roomDoors = gameObject.GetComponentInChildren<RoomDoors>();
        
        //  Subscribe to on room change event 
        _roomDoors.OnRoomChange += OnRoomChange;
    }

    private void Start()
    {
        //  First generate dungeon floor
        GenerateFloor();
        Debug.Log("Floor generated Successfully");

        //  Then load start room
        //  TODO: Remove magic numbers
        int startCell = ((floorObject.floorSize.y / 2) * 10) + (floorObject.floorSize.x / 2);
        var startRoom = floorObject.floorplan[startCell]; 
        Debug.Log("Loading start room");
        LoadRoom(startRoom);
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
            foreach (var (roomIndex, room) in floorObject.floorplan)
            {
                //  Calculate tile position
                int roomX = roomIndex % 10;
                int roomY = roomIndex / 10;
                Vector3Int tilePosition = new Vector3Int(roomX - (floorSize.x / 2), -roomY - floorSize.y / 2, 1);
                //  Starter room
                if (room != null)
                {
                    switch (room)
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
    
    private void LoadRoom(DungeonRoomScriptableObject roomData)
    {
        //  TODO: Move player to correct door location...
        //  Move player to centre
        player.transform.position = Vector3.zero;
        if (roomData != null)
        {
            //  Clear and reset all room tiles
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
            
            //  Set doors
            if (roomData.GetNeighbour(Direction.North) != 0)
            {
                var position = new Vector3Int(0, roomData.RoomBounds.size.y / 2, 0); 
                var neighbourRoom = roomData.GetNeighbour(Direction.North);
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }

            if (roomData.GetNeighbour(Direction.South) != 0)
            {
                var position = new Vector3Int(0, -roomData.RoomBounds.size.y / 2, 0); 
                var neighbourRoom = roomData.GetNeighbour(Direction.South);
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }
            
            if (roomData.GetNeighbour(Direction.East) != 0)
            {
                var position = new Vector3Int(roomData.RoomBounds.size.x / 2, 0, 0); 
                var neighbourRoom = roomData.GetNeighbour(Direction.East);
                _wallsMap.SetTile(position, null);
                _doorMap.SetTile(position, roomData.DoorTile);
                _roomDoors.AddDoor(position, neighbourRoom);
            }
            
            if (roomData.GetNeighbour(Direction.West) != 0)
            {
                var position = new Vector3Int(-roomData.RoomBounds.size.x / 2, 0, 0); 
                var neighbourRoom = roomData.GetNeighbour(Direction.West);
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
