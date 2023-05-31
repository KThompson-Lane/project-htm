using UnityEngine;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "Boss Room", menuName = "Hack the Mainframe/Rooms/Boss Room", order = 2)]
    public class BossRoomScriptableObject : DungeonRoomScriptableObject
    {
        public string BossName;
        public EnemyTile BossTile;
        public EnemySO BossSO;
        public override void InitializeRoom(int level = 1)
        {
            Debug.Log($"BOSS: {BossName}");
        }
    }
}