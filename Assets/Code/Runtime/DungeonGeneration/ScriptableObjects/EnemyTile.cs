﻿using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "New Enemy Tile", menuName = "Tiles/Enemy Tile")]
    public class EnemyTile : Tile
    {
        [SerializeField] private EnemySO enemy;
        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            if (go != null)
            {
                var enemyController = go.GetComponent<EnemyController>();
                enemyController.Initialise(enemy);
            }
            return true;
        }
        public void SetEnemy(EnemySO enemySo) => enemy = enemySo;
    }
}