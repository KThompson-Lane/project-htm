using System.Collections.Generic;
using System.Linq;
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
        //  Once we have a valid floor plan, assign rooms
        //  Make not icky
        var emptyCells = floorplan.Where(cell => cell.Value == null).Select(cell => cell.Key).ToList();
        
        //  Set all our empty cells to a random basic room.
        foreach (var cell in emptyCells)
        {
            floorplan[cell] = BasicRooms[_random.Next(BasicRooms.Count)];
        }
    }

    private DungeonRoomScriptableObject GetRandomRoom()
    {
        var room = BasicRooms[_random.Next(BasicRooms.Count)];
        return Instantiate(room);
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
        floorplan.Add(startCell, Instantiate(StartRoom));
        Cells.Enqueue(startCell);
        
        //  Go over all cells adding neighbours
        while (Cells.Count > 0)
        {
            //  Check neighbouring cells
            int currentCell = Cells.Dequeue();
            addedNeighbour = false;
            
            //  north neighbour
            if (CheckNeighbourConditions(currentCell - 10))
            {
                var room = GetRandomRoom();
                room.southNeighbour = floorplan[currentCell];
                floorplan.Add(currentCell - 10, room);
                floorplan[currentCell].northNeighbour = floorplan[currentCell - 10];
                Cells.Enqueue(currentCell - 10);
                addedNeighbour = true;
            }
            //  south neighbour
            if (CheckNeighbourConditions(currentCell + 10))
            {
                var room = GetRandomRoom();
                room.northNeighbour = floorplan[currentCell];
                floorplan.Add(currentCell + 10, room);
                floorplan[currentCell].southNeighbour = floorplan[currentCell + 10];
                Cells.Enqueue(currentCell + 10);
                addedNeighbour = true;
            }
            //  east neighbour
            if (CheckNeighbourConditions(currentCell + 1))
            {
                var room = GetRandomRoom();
                room.westNeighbour = floorplan[currentCell];
                floorplan.Add(currentCell + 1, room);
                floorplan[currentCell].eastNeighbour = floorplan[currentCell + 1];
                Cells.Enqueue(currentCell + 1);
                addedNeighbour = true;
            }
            //  west neighbour
            if (CheckNeighbourConditions(currentCell - 1))
            {
                var room = GetRandomRoom();
                room.eastNeighbour = floorplan[currentCell];
                floorplan.Add(currentCell - 1, room);
                floorplan[currentCell].westNeighbour = floorplan[currentCell - 1];
                Cells.Enqueue(currentCell - 1);
                addedNeighbour = true;
            }
            if(!addedNeighbour)
                endRooms.Add(currentCell);
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
        floorplan[bossCell] = Instantiate(bossRoom);
        return true;
    }
    private bool CheckNeighbourConditions(int cell)
    {
        if (cell % 10 == 0 || cell < 0)
            return false;
        if (floorplan.ContainsKey(cell))
            return false;
        if (filledNeighbours(cell) > 1)
            return false;
        if (floorplan.Count >= rooms)
            return false;
        if (Random.Range(0, 2) == 1)
            return false;
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
