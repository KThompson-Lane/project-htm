using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class DungeonFloor : MonoBehaviour
{
    // Start is called before the first frame update
    public DungeonFloorScriptableObject floorObject;
    public Tilemap floorPreview;
    public TileBase normalRoomTile;
    public TileBase startRoomTile;
    public TileBase bossRoomTile;

    public void GenerateFloor()
    {
        if (floorObject != null)
            floorObject.GenerateFloor();
        if (floorPreview != null)
        {
            var floorSize = floorObject.floorSize;
            floorPreview.size = new Vector3Int(floorSize.x, floorSize.y, 1);
            floorPreview.origin = new Vector3Int(0, 0, 0);
            floorPreview.ResizeBounds();
            foreach (var room in floorObject.floorplan)
            {
                //  Calculate tile position
                int roomPosition = room.Key;
                int roomX = roomPosition % 10;
                int roomY = roomPosition / 10;
                Vector3Int tilePosition = new Vector3Int(roomX - (floorSize.x / 2), roomY - floorSize.y / 2, 1);
                //  Starter room
                if (room.Value != null)
                {
                    switch (room.Value)
                    {
                        case BossRoomScriptableObject:
                            floorPreview.SetTile(tilePosition, bossRoomTile);
                            break;
                        case StartRoomScriptableObject:
                            floorPreview.SetTile(tilePosition, startRoomTile);
                            break;
                        default:
                            floorPreview.SetTile(tilePosition, normalRoomTile);
                            break;
                    }
                }
            }
            floorPreview.CompressBounds();
        }
    }

    public void Debug()
    {
        UnityEngine.Debug.Log("Debugging");
    }

}
