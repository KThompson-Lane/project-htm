using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "New Map Tile", menuName = "Tiles/Map Tile")]
    public class MapTile : Tile
    {
        //  Sprites for when the tile is hidden, incomplete or cleared
        [SerializeField] private Sprite hiddenSprite, incompleteSprite, clearedSprite;
        private bool _cleared = false, _hidden = true;
        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            base.GetTileData(location, tileMap, ref tileData);
            //    Change Sprite based on if door is open
            tileData.sprite = _hidden? hiddenSprite : _cleared? clearedSprite : incompleteSprite;
        }
        public void Clear() => _cleared = true;
        public void Discover() => _hidden = false;
    }
}