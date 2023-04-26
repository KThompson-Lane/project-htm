using UnityEngine;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "Boss Room", menuName = "Hack the Mainframe/Rooms/Boss Room", order = 2)]
    public class BossRoomScriptableObject : DungeonRoomScriptableObject
    {
        public string BossName;
        public EnemyTile BossTile;
        public EnemySO BossSO;
        //TODO:
            //TBD
            public override void InitializeRoom()
            {
                Debug.Log($"BOSS: {BossName}");
            }
    }
}