using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A class which handles loading in DungeonRoom scriptable objects
/// </summary>
public class DungeonRoom : MonoBehaviour
{
    [field: SerializeField]
    public DungeonRoomScriptableObject RoomData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        LoadRoom();
    }
    
    //TODO: adapt the room when loading such that doors can be placed
    public void LoadRoom()
    {
        var mTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        var mFloorMap = mTilemaps.First(map => map.name == "Floor");
        var mWallsMap = mTilemaps.First(map => map.name == "Walls");
        var mDoorsMap = mTilemaps.First(map => map.name == "Doors");
        
        if (RoomData != null)
        {
            mFloorMap.ClearAllTiles();
            mWallsMap.ClearAllTiles();
            mDoorsMap.ClearAllTiles();
            
            mFloorMap.origin = RoomData.RoomBounds.position;
            mFloorMap.size = RoomData.RoomBounds.size;
            mFloorMap.ResizeBounds();
            mFloorMap.FloodFill(Vector3Int.zero, RoomData.FloorTile);
            
            mWallsMap.origin = RoomData.RoomBounds.position;
            mWallsMap.size = RoomData.RoomBounds.size;
            mWallsMap.ResizeBounds();
            mWallsMap.FloodFill(Vector3Int.zero, RoomData.WallTile);
            
            mDoorsMap.origin = RoomData.RoomBounds.position;
            mDoorsMap.size = RoomData.RoomBounds.size;
            mDoorsMap.ResizeBounds();
            
            //  Fill in doors
            if (RoomData.northNeighbour != 0)
            {
                mWallsMap.SetTile(new Vector3Int(0, RoomData.RoomBounds.size.y/2, 0), null);
                mDoorsMap.SetTile(new Vector3Int(0, RoomData.RoomBounds.size.y/2, 0), RoomData.DoorTile);
            }

            if (RoomData.southNeighbour != 0)
            {
                mWallsMap.SetTile(new Vector3Int(0, -RoomData.RoomBounds.size.y/2, 0), null);
                mDoorsMap.SetTile(new Vector3Int(0, -RoomData.RoomBounds.size.y/2, 0), RoomData.DoorTile);
            }
            
            if (RoomData.eastNeighbour != 0)
            {
                mWallsMap.SetTile(new Vector3Int(RoomData.RoomBounds.size.x/2, 0, 0), null);
                mDoorsMap.SetTile(new Vector3Int(RoomData.RoomBounds.size.x/2,0, 0), RoomData.DoorTile);
            }
            
            if (RoomData.westNeighbour != 0)
            {
                mWallsMap.SetTile(new Vector3Int(-RoomData.RoomBounds.size.x/2,0, 0), null);
                mDoorsMap.SetTile(new Vector3Int(-RoomData.RoomBounds.size.x/2,0, 0), RoomData.DoorTile);
            }
        }
    }
}