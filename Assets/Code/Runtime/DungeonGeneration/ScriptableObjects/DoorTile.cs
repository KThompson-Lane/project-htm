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

        private void OnEnable()
        {
            this.gameObject = DoorPrefab;
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
            this.sprite = OpenSprite;
        }
        public void CloseDoor()
        {
            this.sprite = ClosedSprite;
        }
    }
}