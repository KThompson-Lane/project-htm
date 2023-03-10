using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //  top left cell is 0,1, bottom right is y-1, x
    public Vector2Int floorSize = new(7, 7);
    public int level;
    public int rooms;
    public DungeonRoomScriptableObject StartRoom;
    public List<DungeonRoomScriptableObject> BasicRooms;
    public List<DungeonRoomScriptableObject> BossRooms;
    
    public Dictionary<int, DungeonRoomScriptableObject> floorplan;
    private Queue<int> Cells;
    
    private bool addedNeighbour = false;
    private System.Random _random;
    public void GenerateFloor()
    {
        _random = new System.Random();
        bool validFloor = false;
        while (!validFloor)
            validFloor = CreateFloorPlan();
    }

    private bool CreateFloorPlan()
    {
        floorplan = new();
        Cells = new();
        
        //  Determine number of rooms
        //  TODO: Change formula
        rooms = Random.Range(1, 3) + 5 +(int) (level * 2.6);
        List<int> endRooms = new List<int>();
        
        //  Place our starting cell (2,3 (middle)) into the queue
        int startCell = ((floorSize.y / 2) * 10) + (floorSize.y / 2);
        floorplan.Add(startCell, StartRoom);
        Cells.Enqueue(startCell);
        
        //  Go over all cells adding neighbours
        while (Cells.Count > 0)
        {
            //  Check neighbouring cells
            int currentCell = Cells.Peek();
            addedNeighbour = false;
            //  Top neighbour
            CheckNeighbourConditions(currentCell - 10);
            //  Bottom neighbour
            CheckNeighbourConditions(currentCell + 10);
            //  Right neighbour
            CheckNeighbourConditions(currentCell + 1);
            //  Left neighbour
            CheckNeighbourConditions(currentCell - 1);
            if(!addedNeighbour)
                endRooms.Add(currentCell);
            Cells.Dequeue();
        }

        //  Ensure we made a valid floor
        //TODO: MAKE THIS NOT ICKY
        //  Find our boss room and make sure it isn't neighbouring with start room
        int bossCell = endRooms.Last();
        if (floorplan.Count != rooms) return false;
        if (startCell + 10 == bossCell) return false;
        if (startCell - 10 == bossCell) return false;
        if (startCell + 1 == bossCell) return false;
        if (startCell - 1 == bossCell) return false;
        
        var bossRoom = BossRooms[_random.Next(BossRooms.Count)];
        floorplan[bossCell] = bossRoom;
        return true;
    }
    private void CheckNeighbourConditions(int cell)
    {
        if (cell % 10 == 0 || cell < 0)
            return;
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
