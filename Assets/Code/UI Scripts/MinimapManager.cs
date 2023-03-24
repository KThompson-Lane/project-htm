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
        floorScript.OnRoomChange += OnPlayerMove;
        floorScript.OnRoomCleared += OnRoomClear;
    }

    public void LoadMap(DungeonFloorScriptableObject floorObject)
    {
        if (_dungeonFloorMap != null)
        {
            _floorSize = floorObject.floorSize;
            _dungeonFloorMap.ClearAllTiles();
            foreach (var (roomIndex, room) in floorObject.floorplan)
            {
                //  Calculate tile position
                int roomX = roomIndex % 10;
                int roomY = roomIndex / 10;
                Vector3Int tilePosition = new Vector3Int(roomX - _floorSize.x/2, -roomY + _floorSize.y/2, 1);
                //  Starter room
                if (room != null)
                {
                    switch (room)
                    {
                        case BossRoomScriptableObject:
                            _dungeonFloorMap.SetTile(tilePosition, Instantiate(bossRoomTile));
                            break;
                        case StartRoomScriptableObject:
                            _dungeonFloorMap.SetTile(tilePosition, Instantiate(startRoomTile));
                            _dungeonFloorMap.origin = tilePosition;
                            break;
                        default:
                            _dungeonFloorMap.SetTile(tilePosition, Instantiate(normalRoomTile));
                            break;
                    }
                }
            }
            _dungeonFloorMap.CompressBounds();
        }
    }

    private void OnRoomClear(int room)
    {
        int roomX = room % 10;
        int roomY = room / 10;
        Vector3Int tilePosition = new Vector3Int(roomX - _floorSize.x/2, -roomY + _floorSize.y/2, 1);

        _dungeonFloorMap.GetTile<MapTile>(tilePosition).Cleared = true;
        _dungeonFloorMap.RefreshTile(tilePosition);
    }
    private void OnPlayerMove(int newRoom)
    {
        int roomX = newRoom % 10;
        int roomY = newRoom / 10;
        Vector3Int tilePosition = new Vector3Int(roomX - _floorSize.x/2, -roomY + _floorSize.y/2, 1);
        _playerMapIcon.position = _dungeonFloorMap.CellToWorld(tilePosition);
    }
}
