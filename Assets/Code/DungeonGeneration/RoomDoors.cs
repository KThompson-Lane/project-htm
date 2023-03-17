using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class RoomDoors : MonoBehaviour
{
    // Update is called once per frame
    private Tilemap map;
    private List<KeyValuePair<Vector3Int, int>> _doors;

    public delegate void ChangeRoom(int room);

    public event ChangeRoom OnRoomChange;
    
    private void Awake()
    {
        map = GetComponent<Tilemap>();
        Debug.Log("Test");
        _doors = new();
        
    }

    public void AddDoor(Vector3Int cellPosition, int neighbourRoom)
    {
        _doors.Add(new KeyValuePair<Vector3Int, int>(cellPosition, neighbourRoom));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player"))
            return;
        var cellPosition = map.WorldToCell(col.transform.position);
        var closestDoor = _doors.OrderBy(door => Vector3Int.Distance(cellPosition, door.Key)).First();
        _doors.Clear();
        OnRoomChange?.Invoke(closestDoor.Value);
    }
}
