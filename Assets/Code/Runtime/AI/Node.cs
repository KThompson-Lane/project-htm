using UnityEngine;

namespace Code.Runtime.AI
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public Vector2Int gridIndex;
        public int gCost;
        public int hCost;
        public int fCost => hCost + gCost;
        public Node parent;

        public Node(bool _walkable, Vector3 _worldPos, Vector2Int index)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridIndex = index;
        }

        public int CompareTo(Node other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(other.hCost);
            return -compare;
        }

        public int HeapIndex { get; set; }
    }
    public static class NodeExtensions
    {
        public static int Distance(this Node a, Node b)
        {
            int dstX = Mathf.Abs(a.gridIndex.x - b.gridIndex.x);
            int dstY = Mathf.Abs(a.gridIndex.y - b.gridIndex.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}