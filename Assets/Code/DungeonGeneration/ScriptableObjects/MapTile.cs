using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "New Map Tile", menuName = "Tiles/Map Tile")]
    public class MapTile : Tile
    {
        [SerializeField] private Sprite ClearedSprite;
        [SerializeField] private Sprite BaseSprite; //  Sprite for the tile when it is not yet cleared

        public bool Cleared;

        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            base.GetTileData(location, tileMap, ref tileData);
            //    Change Sprite based on if door is open
            tileData.sprite = Cleared? ClearedSprite : BaseSprite;
        }
    }
}