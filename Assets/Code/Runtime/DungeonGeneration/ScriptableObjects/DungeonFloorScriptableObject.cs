using System.Collections.Generic;
using System.Linq;
using Code.DungeonGeneration;
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
    public List<NormalRoomScriptableObject> BasicRooms;
    public List<BossRoomScriptableObject> BossRooms;
    
    public Dictionary<RoomIndex, DungeonRoomScriptableObject> floorplan;
    private RoomIndex _startRoom;
    private Queue<RoomIndex> Cells;
    
    private bool addedNeighbour = false;
    public DungeonRoomScriptableObject GetStartRoom => floorplan[_startRoom];

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
        InitialiseRooms();
    }

    private void InitialiseRooms()
    {
        //  Initialise Rooms!
        foreach (var (_, room) in floorplan)
        {
            room.InitializeRoom();
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
        var endRooms = new List<RoomIndex>();
        
        //  Place our starting cell (2,3 (middle)) into the queue
        RoomIndex startCell = new RoomIndex(floorSize.x / 2 , floorSize.y / 2);
        AddRoom(startCell, Instantiate(StartRoom));
        _startRoom = startCell;
        Cells.Enqueue(startCell);
        
        //  Go over all cells adding neighbours
        while (Cells.Count > 0)
        {
            //  Check neighbouring cells
            var currentCell = Cells.Dequeue();
            addedNeighbour = false;
            
            //  Check if we can place a neighbour to north
            if (CheckNeighbourConditions(currentCell.North()))
            {
                var room = GetRandomRoom();
                room.SetNeighbour(Direction.South, currentCell);
                AddRoom(currentCell.North(), room);
                floorplan[currentCell].SetNeighbour(Direction.North, currentCell.North());
                Cells.Enqueue(currentCell.North());
                addedNeighbour = true;
            }
            //  south neighbour
            if (CheckNeighbourConditions(currentCell.South()))
            {
                var room = GetRandomRoom();
                room.SetNeighbour(Direction.North, currentCell);
                AddRoom(currentCell.South(), room);
                floorplan[currentCell].SetNeighbour(Direction.South, currentCell.South());
                Cells.Enqueue(currentCell.South());
                addedNeighbour = true;
            }
            //  east neighbour
            if (CheckNeighbourConditions(currentCell.East()))
            {
                var room = GetRandomRoom();
                room.SetNeighbour(Direction.West, currentCell);
                AddRoom(currentCell.East(), room);
                floorplan[currentCell].SetNeighbour(Direction.East, currentCell.East());
                Cells.Enqueue(currentCell.East());
                addedNeighbour = true;
            }
            //  west neighbour
            if (CheckNeighbourConditions(currentCell.West()))
            {
                var room = GetRandomRoom();
                room.SetNeighbour(Direction.East, currentCell);
                AddRoom(currentCell.West(), room);
                floorplan[currentCell].SetNeighbour(Direction.West, currentCell.West());
                Cells.Enqueue(currentCell.West());
                addedNeighbour = true;
            }
            if(!addedNeighbour)
                endRooms.Add(currentCell);
        }

        //  Ensure we made a valid floor
        //  Check to ensure our floor has enough rooms
        if (floorplan.Count != rooms) return false;
        //  Find our boss room and make sure it isn't neighbouring with start room
        var bossCell = endRooms.Last();
        if (floorplan[bossCell].Neighbours.Any(pair => pair.Value == startCell))
            return false;
        
        var bossRoom = Instantiate(BossRooms[_random.Next(BossRooms.Count)]);
        bossRoom.Neighbours = floorplan[bossCell].Neighbours;
        bossRoom.Index = floorplan[bossCell].Index;
        floorplan[bossCell] = bossRoom;
        return true;
    }

    private void AddRoom(RoomIndex index, DungeonRoomScriptableObject room)
    {
        room.Index = index;
        floorplan.Add(index, room);
    }
    private bool CheckNeighbourConditions(RoomIndex cell)
    {
        if (cell.X < 0 || cell.Y <= 0)
            return false;
        if (floorplan.ContainsKey(cell))
            return false;
        if (FilledNeighbours(cell) > 1)
            return false;
        if (floorplan.Count >= rooms)
            return false;
        if (Random.Range(0, 2) == 1)
            return false;
        return true;
    }

    private int FilledNeighbours(RoomIndex cell)
    {
        int filled = 0;
        //  North neighbour
        if(floorplan.ContainsKey(cell.North()))
            filled++;
        //  South neighbour
        if(floorplan.ContainsKey(cell.South()))
            filled++;
        //  East neighbour
        if(floorplan.ContainsKey(cell.East()))
            filled++;
        //  West neighbour
        if(floorplan.ContainsKey(cell.West()))
            filled++;
        return filled;
    }
}
