using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Runtime.AI
{
    public class Pathfinding : MonoBehaviour
    {
        public AStarGrid grid;
        private void Awake()
        {
            grid = GetComponent<AStarGrid>();
        }

        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            Vector3[] waypoints = Array.Empty<Vector3>();
            bool success = false; 
        
            Node startNode = grid.NodeFromWorld(request.pathStart);
            Node targetNode = grid.NodeFromWorld(request.pathEnd);

            if (startNode.walkable && targetNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);
                    if (currentNode == targetNode)
                    {
                        success = true;
                        break;
                    }

                    foreach (var neighbour in grid.GetNeighbours(currentNode))
                    {
                        if(!neighbour.walkable || closedSet.Contains(neighbour))
                            continue;
                        var newMoveCostToNeighbour = currentNode.gCost + currentNode.Distance(neighbour);
                        if (newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMoveCostToNeighbour;
                            neighbour.hCost = neighbour.Distance(targetNode);
                            neighbour.parent = currentNode;
                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        
                        }
                    }
                }
            }
            if (success)
            {
                waypoints = RetracePath(startNode, targetNode);
                success = waypoints.Length > 0;
            }

            callback(new PathResult(waypoints, success, request.callback));
        }

        Vector3[] RetracePath(Node start, Node end)
        {
            List<Node> path = new();
            Node currentNode = end;
            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            var waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new();
            var directionOld = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                var directionNew = new Vector2(path[i - 1].gridIndex.x - path[i].gridIndex.x,
                    path[i - 1].gridIndex.y - path[i].gridIndex.y);
                if(directionNew != directionOld)
                    waypoints.Add(path[i].worldPosition);
                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }
    }
}
