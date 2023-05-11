using System;
using System.Collections.Generic;
using System.Linq;
using Code.DungeonGeneration;
using Code.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

[RequireComponent(typeof(Grid))]
public class DungeonFloor : MonoBehaviour
{
    public Transform player;
    public DungeonFloorScriptableObject floorObject;
    public GameObject pickupPrefab;
    public delegate void RoomChange(RoomIndex newRoom, Direction entryDirection);

    public delegate void RoomClear(RoomIndex cleared);
    public event RoomChange OnRoomChange;
    public event RoomClear OnRoomCleared;
    private DungeonRoomScriptableObject _currentRoom;
    
    private Tilemap _floorMap, _wallsMap;
    List<Vector3Int> _doorPositions = new();
    private Door[] _doors;
    
    public DungeonRoomScriptableObject CurrentRoom =>  _currentRoom;
    public DungeonRoomScriptableObject GetRoom(RoomIndex index) => floorObject.floorplan[index];

    private int _enemiesRemaining;
    
    [NonSerialized] public UnityEvent<bool> RoomClearedEvent;
    [NonSerialized] public UnityEvent EnemyKilledEvent;

    [SerializeField] private Light2DBase roomLight;
    
    private void Awake()
    {
        //  Get tilemaps and room doors script
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
        
        //  First generate dungeon floor
        GenerateFloor();
        Debug.Log("Floor generated Successfully");

        //  Then load start room
        var startRoom = floorObject.GetStartRoom;
        Debug.Log("Loading start room");
        LoadRoom(startRoom);
        
        // Initialise events
        RoomClearedEvent ??= new UnityEvent<bool>();
        EnemyKilledEvent ??= new UnityEvent();
    }
    public void GenerateFloor()
    {
        if (floorObject != null)
            floorObject.GenerateFloor();
    }

    public void ChangeRoom(RoomIndex index)
    {
        LoadRoom(floorObject.floorplan[index]);
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
        foreach (var (tile, position) in _currentRoom.GetFloorTiles())
        {
            _floorMap.SetTile(position, tile);
        }
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

        //  Create room doors
        CreateDoors();

        roomLight.enabled = _currentRoom.Cleared;

        if(!_currentRoom.Cleared)
            PlaceEnemies();
        //  Create hazards

        //  Load newly created door objects and subscribe to their events
        _doors = GetComponentsInChildren<Door>();
        foreach (var door in _doors)
        {
            door.OnDoorTrigger += OnDoorTriggered;
        }
    }

    private void PlaceEnemies()
    {
        switch (_currentRoom)
        {
            case NormalRoomScriptableObject room:
            {
                foreach (var (position, enemy) in room.GetEnemies())
                {
                    var enemyTile = Instantiate(room.EnemyTile);
                    enemyTile.SetEnemy(enemy);
                    _floorMap.SetTile(position, enemyTile);
                    _enemiesRemaining++;
                }

                break;
            }
            case BossRoomScriptableObject room:
                var bossTile = Instantiate(room.BossTile);
                bossTile.SetEnemy(room.BossSO);
                _floorMap.SetTile(Vector3Int.RoundToInt(room.RoomBounds.center), bossTile);
                _enemiesRemaining++;
                break;
        }

        foreach (var enemy in GetComponentsInChildren<EnemyController>())
        {
            enemy.OnDie += OnEnemyKilled;
        }
    }

    private void CreateDoors()
    {
        //  Set doors
        if (_currentRoom.GetNeighbour(Direction.North) != null)
        {
            var position = new Vector3Int(0, _currentRoom.RoomBounds.size.y / 2, 0);
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.NorthDoor);
        }

        if (_currentRoom.GetNeighbour(Direction.East) != null)
        {
            var position = new Vector3Int(_currentRoom.RoomBounds.size.x / 2, 0, 0); 
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.EastDoor);
        }
            
        if (_currentRoom.GetNeighbour(Direction.South) != null)
        {
            var position = new Vector3Int(0, -_currentRoom.RoomBounds.size.y / 2, 0); 
            _doorPositions.Add(position);
            _wallsMap.SetTile(position, _currentRoom.SouthDoor);
        }
            
        if (_currentRoom.GetNeighbour(Direction.West) != null)
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
    //  This should be moved to Game manager
    private void OnDoorTriggered(Direction direction)
    {
        if (!_currentRoom.Cleared) return;
        var newRoomIndex = _currentRoom.GetNeighbour(direction) ?? 
                           throw new Exception("Invalid room");
        //  Unsubscribe from our previous room
        _currentRoom.OnRoomCleared -= OnRoomClear;
        OnRoomChange?.Invoke(newRoomIndex, direction);
    }

    public void MovePlayer(Direction entranceDirection)
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

    private void OnEnemyKilled(Vector3 deathPosition)
    {
        EnemyKilledEvent.Invoke();
        RollDrops(deathPosition);
        if (--_enemiesRemaining != 0) return;
        Debug.Log("clearing room");
        _currentRoom.Cleared = true;
    }

    private void RollDrops(Vector3 deathPosition)
    {
        if (_currentRoom is NormalRoomScriptableObject room)
        {
            var cellPosition = _floorMap.WorldToCell(deathPosition);
            var dropped = room.RollPickups(_floorMap.WorldToCell(cellPosition));
            if (dropped)
            {
                //  place drop
                var (dropPosition,drop) = room.GetPickups().Last();

                var pickup = ObjectPooler.SharedInstance.GetPooledObject("Pickup");
                pickup.transform.position = _floorMap.GetCellCenterWorld(dropPosition);
                pickup.GetComponent<Pickup>().SetPickup(drop);
                pickup.SetActive(true);
            }
        }
    }
    
    private void OnRoomClear()
    {
        var bossRoom = false;
        foreach (var doorPosition in _doorPositions)
        {
            _wallsMap.GetTile<DoorTile>(doorPosition).OpenDoor();
            _wallsMap.RefreshTile(doorPosition);
        }

        if (_currentRoom is BossRoomScriptableObject boss)
        {
            bossRoom = true;
            Debug.Log($"Boss {boss.BossName} killed!");
        }
        OnRoomCleared?.Invoke(_currentRoom.Index);
        RoomClearedEvent.Invoke(bossRoom);
        roomLight.enabled = true;
    }
#if UNITY_EDITOR
    public void PreviewRoom()
    {
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        _floorMap = mTilemaps.First(map => map.name == "Floor");
        _wallsMap = mTilemaps.First(map => map.name == "Walls");
        var startRoom = floorObject.GetStartRoom;
        this.LoadRoom(startRoom);
    }
    //  TEST METHOD FOR CLEARING ROOMS
    public void ClearRoom()
    {
        _currentRoom.Cleared = true;
    }
#endif
}
