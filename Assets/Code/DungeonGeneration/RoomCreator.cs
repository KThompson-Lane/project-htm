using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
public class RoomCreator : EditorWindow
{
    private string roomName = "My Room";
    private Vector2Int roomSize = new(5,5);

    private RuleTile _floorTile;
    private RuleTile _wallTile;
    private GameObject _active;
    private bool _generated;
    
    [MenuItem("Window/Create Dungeon Room")]
    public static void ShowWindow()
    {
        GetWindow<RoomCreator>();
    }

    private void OnGUI()
    {
        GUILayout.Label ("Room Properties", EditorStyles.boldLabel);
        roomName = EditorGUILayout.TextField ("Room name", roomName);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Room Width");
        GUILayout.Label("Room Height");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        roomSize.x = EditorGUILayout.IntSlider(roomSize.x, 5, 13);
        roomSize.y = EditorGUILayout.IntSlider(roomSize.y, 5, 13);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        
        _floorTile = (RuleTile) EditorGUILayout.ObjectField("Floor tile", _floorTile, typeof(RuleTile), true);
        _wallTile = (RuleTile) EditorGUILayout.ObjectField("Wall tile", _wallTile, typeof(RuleTile), true);

        if (GUILayout.Button("Generate Room"))
        {
            _active = Selection.activeGameObject;
            if (_active == null)
            {
                Debug.LogWarning("Select room preview before generating room");
                return;
            }
            if (!_active.CompareTag("PreviewRoom"))
            {
                Debug.LogWarning("Select room preview before generating room");
                return;
            }
            CreateRoom();
            _generated = true;
        }

        if (_generated)
        {
            if(GUILayout.Button("Save room"))
            {
                SaveRoom();
                _generated = false;
            }
        }
    }

    
    //  TODO: Add support for an interactables layer
    private void CreateRoom()
    {
        var mTilemaps = _active.GetComponentsInChildren<Tilemap>();
        var mFloorMap = mTilemaps.First(map => map.name == "Floor");
        var mWallsMap = mTilemaps.First(map => map.name == "Walls");

        mFloorMap.ClearAllTiles();
        mFloorMap.size = new Vector3Int(roomSize.x, roomSize.y, 1);
        mFloorMap.origin = new Vector3Int(-roomSize.x/2, -roomSize.y/2, 0);
        mFloorMap.ResizeBounds();
        mFloorMap.FloodFill(Vector3Int.zero, _floorTile);
        
        mWallsMap.ClearAllTiles();                                                    
        mWallsMap.size = new Vector3Int(roomSize.x, roomSize.y, 1);
        mWallsMap.origin = new Vector3Int(-roomSize.x/2, -roomSize.y/2, 0);
        mWallsMap.ResizeBounds();
        mWallsMap.FloodFill(Vector3Int.zero, _wallTile);     
    }

    private void SaveRoom()
    {
        var dungeonRoomObject = CreateInstance<DungeonRoomScriptableObject>();                          
        dungeonRoomObject.Init(roomName, _floorTile, _wallTile, roomSize);                                      
        //var tiles = mWallMap.GetTilesBlock(mWallMap.cellBounds);                                                       
        AssetDatabase.CreateAsset(dungeonRoomObject, $"Assets/Level/Dungeon Rooms/{roomName}.asset");                    
        AssetDatabase.SaveAssets();                                                                                      
    }
}
#endif
