using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "Normal Room", menuName = "Hack the Mainframe/Rooms/Normal Room", order = 1)]
    public class NormalRoomScriptableObject : DungeonRoomScriptableObject
    {
        public List<EnemySO> EnemyPool;
        public List<PickupSO> PickupPool;
        
        public EnemyTile EnemyTile;

        private int maxEnemies;
        private int dropChance;
        
        //  Lists for storing the positions of enemies and pickups
        private List<Tuple<Vector3Int, EnemySO>>_enemies;
        public List<Tuple<Vector3Int, EnemySO>> GetEnemies() => _enemies;
        
        private Dictionary<Vector3Int, PickupSO> _pickups;
        public Dictionary<Vector3Int, PickupSO> GetPickups() => _pickups;

        public void RemovePickup(Vector3Int position) => _pickups.Remove(position);

        //  Add obstacles / other data
        public override void InitializeRoom(int level = 1)
        {
            _enemies = new List<Tuple<Vector3Int, EnemySO>>();
            _pickups = new Dictionary<Vector3Int, PickupSO>();
            
            //  Use level to calculate drop rate and max enemies
            maxEnemies = 3 + (int) (level * 2.6);
            dropChance = 10 + (int) (level * 3.45);
            
            //  Generate list of possible positions
            var possibleLocations = layout.FloorTiles.Tiles.Where(x => (x.Tile is not RuleTile)).SelectMany(x => x.Positions);
            possibleLocations =
                possibleLocations.Where(position =>
                    (position.x != layout.RoomBounds.xMax && position.x != layout.RoomBounds.xMin) &&
                    (position.y != (layout.RoomBounds.yMax-1) && position.y != layout.RoomBounds.yMin));
            
            foreach (var enemyPosition in possibleLocations.OrderBy(_ => Guid.NewGuid()))
            {
                //  Check conditions
                
                //  Not already max enemies
                if(_enemies.Count == maxEnemies)
                    break;
                //  No neighbouring enemies
                if (_enemies.Any((pos) => pos.Item1.x == enemyPosition.x || pos.Item1.y == enemyPosition.y))
                    continue;
                //  One in 3 chance of giving up 
                if (Random.Range(0, 4) < 3)
                    continue;
                //  Add enemy to list
                _enemies.Add(new (enemyPosition, EnemyPool[Random.Range(0,EnemyPool.Count)]));
            }
        }

        public bool RollPickups(Vector3Int dropPosition)
        {
            if (PickupPool.Count == 0)
                return false;
            //  Roll whether to place a pickup
            var roll = Random.Range(0, 100);
            return roll < dropChance && _pickups.TryAdd(dropPosition, PickupPool[Random.Range(0, PickupPool.Count)]);
        }
    }
}