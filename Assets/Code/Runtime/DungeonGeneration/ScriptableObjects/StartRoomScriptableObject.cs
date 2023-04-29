using UnityEngine;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "Start Room", menuName = "Hack the Mainframe/Rooms/Start Room", order = 3)]
    public class StartRoomScriptableObject : DungeonRoomScriptableObject
    {
        public string levelMessage;
        //TODO:
        //Add start room specific things
        //TBD
        public override void InitializeRoom()
        {
            Debug.Log(levelMessage);
        }
    }
}