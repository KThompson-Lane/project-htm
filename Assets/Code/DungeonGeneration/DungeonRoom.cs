using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    [field: SerializeField]
    public DungeonRoomScriptableObject RoomData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        var mFloorMap = mTilemaps.First(map => map.name == "Floor");
        var mWallsMap = mTilemaps.First(map => map.name == "Walls");

        if (RoomData != null)
        {
            mFloorMap.ClearAllTiles();
            mWallsMap.ClearAllTiles();
            
            mFloorMap.origin = RoomData.RoomBounds.position;
            mFloorMap.size = RoomData.RoomBounds.size;
            mFloorMap.ResizeBounds();
            mFloorMap.FloodFill(Vector3Int.zero, RoomData.FloorTile);
            
            mWallsMap.origin = RoomData.RoomBounds.position;
            mWallsMap.size = RoomData.RoomBounds.size;
            mWallsMap.ResizeBounds();
            mWallsMap.FloodFill(Vector3Int.zero, RoomData.WallTile);
        }
    }
}
