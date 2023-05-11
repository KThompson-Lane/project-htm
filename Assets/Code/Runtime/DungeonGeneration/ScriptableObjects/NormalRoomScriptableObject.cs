﻿using System;
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

        public int MaxEnemies;
        //  Lists for storing the positions of enemies and pickups
        private List<Tuple<Vector3Int, EnemySO>>_enemies;
        public List<Tuple<Vector3Int, EnemySO>> GetEnemies() => _enemies;
        
        private Dictionary<Vector3Int, PickupSO> _pickups;
        public Dictionary<Vector3Int, PickupSO> GetPickups() => _pickups;

        public void RemovePickup(Vector3Int position) => _pickups.Remove(position);
        

        
        //  Add obstacles / other data
        public override void InitializeRoom()
        {
            //throw new System.NotImplementedException();
            _enemies = new List<Tuple<Vector3Int, EnemySO>>();
            _pickups = new Dictionary<Vector3Int, PickupSO>();
            //  Generate list of possible positions
            
            var possibleLocations = FloorTiles.Tiles.Where(x => (x.Tile is not RuleTile)).SelectMany(x => x.Positions);
            possibleLocations =
                possibleLocations.Where(position =>
                    (position.x != RoomBounds.xMax && position.x != RoomBounds.xMin) &&
                    (position.y != (RoomBounds.yMax-1) && position.y != RoomBounds.yMin));
            
            foreach (var enemyPosition in possibleLocations.OrderBy(_ => Guid.NewGuid()))
            {
                //  Check conditions
                
                //  Not already max enemies
                if(_enemies.Count == MaxEnemies)
                    break;
                //  No neighbouring enemies
                if (_enemies.Any((pos) => pos.Item1.x == enemyPosition.x || pos.Item1.y == enemyPosition.y))
                    continue;
                //  One in 3 chance of giving up 
                //if (Random.Range(0, 4) < 3)
                 //   continue;
                //  Add enemy to list
                _enemies.Add(new (enemyPosition, EnemyPool[Random.Range(0,EnemyPool.Count)]));
            }
        }

        public bool RollPickups(Vector3Int dropPosition)
        {
            //  Roll whether to place a pickup
            var roll = Random.Range(0, 10);
            return roll < PickupPool.Count && _pickups.TryAdd(dropPosition, PickupPool[Random.Range(0, PickupPool.Count)]);
        }
    }
}