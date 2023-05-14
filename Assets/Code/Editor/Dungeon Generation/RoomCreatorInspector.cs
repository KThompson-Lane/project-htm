using Code.DungeonGeneration;
using Code.Runtime.DungeonGeneration;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Dungeon_Generation
{
    [CustomEditor(typeof(RoomCreator))]
    public class RoomCreatorInspector : UnityEditor.Editor
    {
        private string _roomName = "newRoom";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            RoomCreator myScript = (RoomCreator) target;
            _roomName = EditorGUILayout.TextField ("Room name", _roomName);
            if (GUILayout.Button("Create normal room"))
            {
                myScript.CreateRoom();

                var roomObject = CreateInstance<NormalRoomScriptableObject>();
                roomObject.SetTilemaps(myScript.FloorTiles);
                roomObject.RoomBounds = myScript.RoomBounds;
                AssetDatabase.CreateAsset(roomObject, $"Assets/Level/Scriptable Objects/Dungeon Rooms/Standard Rooms/{_roomName}.asset");                    
                AssetDatabase.SaveAssets();         
            }
            if (GUILayout.Button("Create start room"))
            {
                myScript.CreateRoom();

                var roomObject = CreateInstance<StartRoomScriptableObject>();
                roomObject.SetTilemaps(myScript.FloorTiles);
                roomObject.RoomBounds = myScript.RoomBounds;
                AssetDatabase.CreateAsset(roomObject, $"Assets/Level/Scriptable Objects/Dungeon Rooms/Starter Rooms/{_roomName}.asset");                    
                AssetDatabase.SaveAssets();         
            }
            if (GUILayout.Button("Create boss room"))
            {
                myScript.CreateRoom();

                var roomObject = CreateInstance<BossRoomScriptableObject>();
                roomObject.SetTilemaps(myScript.FloorTiles);
                roomObject.RoomBounds = myScript.RoomBounds;
                AssetDatabase.CreateAsset(roomObject, $"Assets/Level/Scriptable Objects/Dungeon Rooms/Boss Rooms/{_roomName}.asset");                    
                AssetDatabase.SaveAssets();         
            }

            if (GUILayout.Button("Preview Dungeon Room"))
            {
                myScript.LoadRoom();
            }
        }
    }

}