using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom inspector which adds a button to reload dungeon rooms
/// </summary>
#if UNITY_EDITOR
[CustomEditor(typeof(DungeonFloor))]
public class DungeonFloorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DungeonFloor myScript = (DungeonFloor)target;
        if(GUILayout.Button("Clear room"))
        {
            myScript.ClearRoom();
        }
        if(GUILayout.Button("Clear floor"))
        {
            myScript.ClearFloor();
        }
        if(GUILayout.Button("Generate floor"))
        {
            myScript.GenerateFloor();
        }
        if(GUILayout.Button("Preview room"))
        {
            myScript.PreviewRoom();
        }

    }
}
#endif