using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A scriptable object which holds data about a given floor
/// Each floor will have a starting room
/// A level which determines the difficulty/number of rooms on a floor
/// A list of regular rooms
/// A list of possible boss rooms for a given difficulty
/// 
/// </summary>
[CreateAssetMenu(fileName = "New Dungeon floor", menuName = "Hack the Mainframe/Dungeon floor", order = 1)]
public class DungeonFloorScriptableObject : ScriptableObject
{
    //  top left cell is 0,1, bottom right is 4,5
    public Vector2Int floorSize = new(7, 7);
    public int level;
    public DungeonRoomScriptableObject StartRoom;
    public List<DungeonRoomScriptableObject> BasicRooms;
    public List<DungeonRoomScriptableObject> BossRooms;
    public Dictionary<int, DungeonRoomScriptableObject> floorplan = new();
    public Queue<int> Cells = new Queue<int>();
    public int rooms;
    public bool addedNeighbour = false;
    public void GenerateFloor()
    {
        //  Determine number of rooms
        //  TODO: Change formula
        rooms = Random.Range(1, 3) + 5 +(int) (level * 2.6);
        List<int> endRooms = new List<int>();
        
        //  Place our starting cell (2,3 (middle)) into the queue
        int startCoordinate = ((floorSize.y / 2) * 10) + (floorSize.y / 2);
        floorplan.Add(startCoordinate, StartRoom);
        Cells.Enqueue(startCoordinate);
        while (Cells.Count > 0)
        {
            //  Check neighbouring cells
            int currentCell = Cells.Peek();
            addedNeighbour = false;
            //  Top neighbour
            CheckConditions(currentCell - 10);
            //  Bottom neighbour
            CheckConditions(currentCell + 10);
            //  Right neighbour
            CheckConditions(currentCell + 1);
            //  Left neighbour
            CheckConditions(currentCell - 1);
            if(!addedNeighbour)
                endRooms.Add(currentCell);
            Cells.Dequeue();
        }
    }

    private void CheckConditions(int cell)
    {
        if (floorplan.ContainsKey(cell))
            return;
        if (filledNeighbours(cell) > 1)
            return;
        if (floorplan.Count >= rooms)
            return;
        if (Random.Range(0, 2) == 1)
            return;
        floorplan.Add(cell, null);
        Cells.Enqueue(cell);
        addedNeighbour = true;
    }

    private int filledNeighbours(int cell)
    {
        int filled = 0;
        //  Top neighbour
        if(floorplan.ContainsKey(cell - 10))
            filled++;
        //  Bottom neighbour
        if(floorplan.ContainsKey(cell + 10))
            filled++;
        //  Right neighbour
        if(floorplan.ContainsKey(cell + 1))
            filled++;
        //  Left neighbour
        if(floorplan.ContainsKey(cell - 1))
            filled++;
        return filled;
    }
}
