using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom inspector which adds a button to reload dungeon rooms
/// </summary>
[CustomEditor(typeof(DungeonRoom))]
public class DungeonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DungeonRoom myScript = (DungeonRoom)target;
        if(GUILayout.Button("Reload room"))
        {
            myScript.LoadRoom();
        }
    }
}