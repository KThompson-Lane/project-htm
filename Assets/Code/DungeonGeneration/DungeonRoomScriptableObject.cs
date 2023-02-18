using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dungeon/Room", order = 1)]
public class DungeonRoomScriptableObject : ScriptableObject
{

    public string RoomName;

    public RuleTile FloorTile;

    public RuleTile WallTile;
    
    public BoundsInt RoomBounds;
    
    public void Init(string roomName, RuleTile floor, RuleTile wall, Vector2Int roomSize)
    {
        RoomName = roomName;
        FloorTile = floor;
        WallTile = wall;
        RoomBounds.size = new Vector3Int(roomSize.x, roomSize.y, 1);
        RoomBounds.position = -(new Vector3Int(roomSize.x, roomSize.y, 1) / 2); 
    }
}
