using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "New Hazard Tile", menuName = "Tiles/Hazard Tile")]
    public class HazardTile : Tile
    {
        [SerializeField] private bool enabled;
        //TODO: Replace flat damage amount w/ a hazard/effect SO giving the hazard tile different effects
        [SerializeField] private int damage;
        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            if (enabled && go != null)
            {
                var hazard = go.AddComponent<Hazard>();
                hazard.SetDamage(damage);
            }
            return true;
        }
    }
}