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
    private const int FloorDimensions = 5 * 5;
    public int level;
    public DungeonRoomScriptableObject StartRoom;
    public List<DungeonRoomScriptableObject> BasicRooms;
    public List<DungeonRoomScriptableObject> BossRooms;
    private Dictionary<int, DungeonRoomScriptableObject> floorplan = new();
    private Queue<int> Cells = new Queue<int>();
    private int rooms;
    
    public void GenerateFloor()
    {
        //  Determine number of rooms
        //  TODO: Change formula
        rooms = Random.Range(1, 3) + 5 +(int) (level * 2.6);

        //  Place our starting cell (2,3 (middle)) into the queue
        floorplan.Add(23, StartRoom);
        Cells.Enqueue(23);
        while (Cells.Count > 0)
        {
            //  Check neighbouring cells
            int currentCell = Cells.Peek();
            
            //  Top neighbour
            CheckConditions(currentCell - 10);
            //  Bottom neighbour
            CheckConditions(currentCell + 10);
            //  Right neighbour
            CheckConditions(currentCell + 1);
            //  Left neighbour
            CheckConditions(currentCell - 1);

        }
    }

    private bool CheckConditions(int cell)
    {
        if (floorplan.ContainsKey(cell))
            return false;
        if (filledNeighbours(cell) > 1)
            return false;
        if (floorplan.Count >= rooms)
            return false;
        if (Random.Range(0, 2) == 1)
            return false;
        floorplan.Add(cell, null);
        return true;
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
