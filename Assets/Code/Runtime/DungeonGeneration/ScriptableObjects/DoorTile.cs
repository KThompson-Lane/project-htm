using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.DungeonGeneration
{
    [CreateAssetMenu(fileName = "New Door Tile", menuName = "Tiles/Door Tile")]
    public class DoorTile : Tile
    {
        [SerializeField] private Sprite OpenSprite;
        [SerializeField] private Sprite ClosedSprite;
        [SerializeField] private Direction direction;
        [SerializeField] private GameObject DoorPrefab;
        
        private bool IsOpen;

        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            base.GetTileData(location, tileMap, ref tileData);
            //    Change Sprite based on if door is open
            tileData.sprite = IsOpen? OpenSprite : ClosedSprite;
            tileData.gameObject = DoorPrefab;
        }
        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            if (go != null)
            {
                var door = go.GetComponent<Door>();
                door.DoorDirection = direction;
            }
            return true;
        }
        public void OpenDoor()
        {
            IsOpen = true;
        }
        public void CloseDoor()
        {
            IsOpen = false;
        }
    }
}