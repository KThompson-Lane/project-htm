using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Runtime.AI
{
    public class AStarGrid : MonoBehaviour
    {
    
        public BoundsInt gridBounds;
        public Tilemap map;
        private Node[,] _grid;
        private int _gridSizeX, _gridSizeY;
        private Vector2 _gridWorldSize;

        public int MaxSize => _gridSizeX * _gridSizeX;
        private void Awake()
        {
            gridBounds = map.cellBounds;
            _gridSizeX = gridBounds.size.x;
            _gridSizeY = gridBounds.size.y;

            _gridWorldSize = new();
            _gridWorldSize.x = gridBounds.size.x * map.cellSize.x;
            _gridWorldSize.y = gridBounds.size.y * map.cellSize.y;
            CreateGrid();
        }

        public void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY];

            Vector3Int worldBottomLeft = gridBounds.min;
        
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    var cellPosition = worldBottomLeft + new Vector3Int(x, y, 0);
                    var worldPosition = map.CellToWorld(cellPosition) + map.tileAnchor;
                    bool walkable = map.GetTile(cellPosition) is not RuleTile;
                    _grid[x, y] = new Node(walkable, worldPosition, new(x,y));
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y ==0)
                        continue;

                    var checkIndex = node.gridIndex + new Vector2Int(x, y);
                    if (checkIndex.x >= 0 && checkIndex.x < _gridSizeX && checkIndex.y >= 0 && checkIndex.y < _gridSizeY)
                    {
                        neighbours.Add(_grid[checkIndex.x, checkIndex.y]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorld(Vector3 worldPos)
        {
            float percentX = (worldPos.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
            float percentY = (worldPos.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            return _grid[x, y];
        }

    }
}
