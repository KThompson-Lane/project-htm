﻿using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "Normal Room", menuName = "Hack the Mainframe/Rooms/Normal Room", order = 1)]
    public class NormalRoomScriptableObject : DungeonRoomScriptableObject
    {
        //TODO: 
        public List<EnemySO> EnemyPool;
        public EnemyTile EnemyTile;

        public int MaxEnemies;
        //  Lists for storing the positions of hazards and enemies
        private List<Tuple<Vector3Int, EnemySO>>_enemies;
        public List<Tuple<Vector3Int, EnemySO>> GetEnemies() => _enemies;

        //  Add list of enemies
        //  Add obstacles / other data
        public override void InitializeRoom()
        {
            //throw new System.NotImplementedException();
            _enemies = new List<Tuple<Vector3Int, EnemySO>>();
            //  Generate list of possible positions

            var possibleLocations = FloorTiles.Tiles.Where(x => (x.Tile is not RuleTile)).SelectMany(x => x.Positions);

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
    }
}