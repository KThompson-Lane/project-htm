using Code.DungeonGeneration;
using Code.Runtime.DungeonGeneration;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Dungeon_Generation
{
    [CustomEditor(typeof(RoomCreator))]
    public class RoomCreatorInspector : UnityEditor.Editor
    {
        private string _layoutName = "basic layout";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            RoomCreator myScript = (RoomCreator) target;
            _layoutName = EditorGUILayout.TextField ("Layout name", _layoutName);
            if (GUILayout.Button("Save room layout"))
            {
                myScript.SaveLayout();

                var roomObject = CreateInstance<RoomLayout>();
                roomObject.SetTilemaps(myScript.FloorTiles);
                roomObject.RoomBounds = myScript.RoomBounds;
                roomObject.NorthDoor = myScript.NorthDoor;
                roomObject.SouthDoor = myScript.SouthDoor;
                roomObject.EastDoor = myScript.EastDoor;
                roomObject.WestDoor = myScript.WestDoor;
                roomObject.WallTile = myScript.WallTile;
                
                AssetDatabase.CreateAsset(roomObject, $"Assets/Level/Scriptable Objects/Dungeon Rooms/Layouts/{_layoutName}.asset");                    
                AssetDatabase.SaveAssets();         
            }
            if (GUILayout.Button("Load room layout"))
            {
                myScript.LoadLayout();
            }

        }
    }

}