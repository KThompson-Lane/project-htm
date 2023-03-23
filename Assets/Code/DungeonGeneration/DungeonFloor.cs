using System.Collections.Generic;
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
    
    private Tilemap _floorMap, _wallsMap;
    List<Vector3Int> _doorPositions = new();
    private Door[] _doors;
    private DungeonRoomScriptableObject _currentRoom;
    private void Awake()
    {
        //  Get tilemaps and room doors script
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
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
    
    private void LoadRoom(DungeonRoomScriptableObject newRoom)
    {
        if (newRoom == null)
        {
            Debug.LogError("Trying to load a null room!");
            return;
        }
        
        //  Set our new room to be our current room
        _currentRoom = newRoom;
        //  Subscribe to new room on clear
        _currentRoom.OnRoomCleared += OnRoomClear;
        //  Clear and reset all room tiles
        _floorMap.ClearAllTiles();
        _wallsMap.ClearAllTiles();
        _doorPositions.Clear();
        _doors = null;
        _floorMap.origin = _currentRoom.RoomBounds.position;
        _floorMap.size = _currentRoom.RoomBounds.size;
        _floorMap.ResizeBounds();
        _floorMap.FloodFill(Vector3Int.zero, _currentRoom.FloorTile);
        Debug.Log(_currentRoom.RoomBounds.yMax);
        Debug.Log(_currentRoom.RoomBounds.xMax);
        for (int x = _currentRoom.RoomBounds.xMin; x < _currentRoom.RoomBounds.xMax; x++)
        {
            _wallsMap.SetTile(new Vector3Int(x, _currentRoom.RoomBounds.yMin), _currentRoom.WallTile);
            _wallsMap.SetTile(new Vector3Int(x, _currentRoom.RoomBounds.yMax-1), _currentRoom.WallTile);
        }
        for (int y = _currentRoom.RoomBounds.yMin; y < _currentRoom.RoomBounds.yMax; y++)
        {
            _wallsMap.SetTile(new Vector3Int(_currentRoom.RoomBounds.xMin, y), _currentRoom.WallTile);
            _wallsMap.SetTile(new Vector3Int(_currentRoom.RoomBounds.xMax-1, y), _currentRoom.WallTile);
        }

        // Create room doors
        CreateDoors();
        
        //  Load newly created door objects and subscribe to their events
        _doors = GetComponentsInChildren<Door>();
        foreach (var door in _doors)
        {
            door.OnDoorTrigger += OnDoorTriggered;
        }
    }

    private void CreateDoors()
    {
        //  Set doors
        if (_currentRoom.GetNeighbour(Direction.North) != 0)
        {
            var position = new Vector3Int(0, _currentRoom.RoomBounds.size.y / 2, 0);
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.NorthDoor);
        }

        if (_currentRoom.GetNeighbour(Direction.East) != 0)
        {
            var position = new Vector3Int(_currentRoom.RoomBounds.size.x / 2, 0, 0); 
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.EastDoor);
        }
            
        if (_currentRoom.GetNeighbour(Direction.South) != 0)
        {
            var position = new Vector3Int(0, -_currentRoom.RoomBounds.size.y / 2, 0); 
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.SouthDoor);
        }
            
        if (_currentRoom.GetNeighbour(Direction.West) != 0)
        {
            var position = new Vector3Int(-_currentRoom.RoomBounds.size.x / 2, 0, 0); 
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.WestDoor);
        }
        foreach (var doorPosition in _doorPositions)
        {
            var doorTile = _wallsMap.GetTile<DoorTile>(doorPosition);
            if(_currentRoom.Cleared)
                doorTile.OpenDoor();
            else
                doorTile.CloseDoor();
            _wallsMap.RefreshTile(doorPosition);
        }
    }
    private void OnDoorTriggered(Direction direction)
    {
        if (!_currentRoom.Cleared) return;
        var newRoom = floorObject.floorplan[_currentRoom.GetNeighbour(direction)];
        //  Unsubscribe from our previous room
        _currentRoom.OnRoomCleared -= OnRoomClear;
        LoadRoom(newRoom);
        //  Move player.
        MovePlayer(direction);
    }

    private void MovePlayer(Direction entranceDirection)
    {
        //  Entrance direction is the direction going *in* to the room,
        // i.e. entering from the north means you are in the south/bottom of the room.
    
        var newLocation = entranceDirection switch
        {
            Direction.North => new Vector3Int(0, (-_currentRoom.RoomBounds.size.y / 2) + 2, 0),
            Direction.South => new Vector3Int(0, (_currentRoom.RoomBounds.size.y / 2) - 2, 0),
            Direction.East => new Vector3Int((-_currentRoom.RoomBounds.size.x / 2) + 2, 0, 0),
            Direction.West => new Vector3Int((_currentRoom.RoomBounds.size.x / 2) - 2, 0, 0),
            _ => Vector3Int.zero
        };
        player.transform.position = _floorMap.CellToWorld(newLocation);
        
    }
    private void OnRoomClear()
    {
        foreach (var doorPosition in _doorPositions)
        {
            _wallsMap.GetTile<DoorTile>(doorPosition).OpenDoor();
            _wallsMap.RefreshTile(doorPosition);
        }
    }
#if UNITY_EDITOR
    public void PreviewRoom()
    {
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
        int startCell = ((floorObject.floorSize.y / 2) * 10) + (floorObject.floorSize.x / 2);
        var startRoom = floorObject.floorplan[startCell]; 
        this.LoadRoom(startRoom);
    }
    //  TEST METHOD FOR CLEARING ROOMS
    public void ClearRoom()
    {
        _currentRoom.Cleared = true;
    }
    public void ClearFloor()
    {
        foreach (var room in floorObject.floorplan.Values)
        {
            room.Cleared = true;
        }
    }
#endif
}
