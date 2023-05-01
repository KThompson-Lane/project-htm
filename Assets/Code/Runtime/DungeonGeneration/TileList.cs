using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Runtime.DungeonGeneration
{
    [Serializable]
    public class TileList
    {
        public List<TileSet> Tiles = new List<TileSet>();

        public TileSet this[TileBase tile]
        {
            get { return Tiles.First(set => set.Tile == tile); }
        }
    }

    public static class TileListExtensions
    {
        public static bool TryAdd(this TileList list, TileSet newSet)
        {
            if (list.Tiles.Any(set => set.Tile == newSet.Tile))
            {
                return false;
            }
            list.Tiles.Add(newSet);
            return true;
        }

        public static IEnumerable<Tuple<TileBase, Vector3Int>> Enumerate(this TileList list)
        {
            return list.Tiles.SelectMany(f => f.Positions.Select(s => new Tuple<TileBase, Vector3Int>(f.Tile, s)));
        }
    }

    [Serializable]
    public struct TileSet
    {
        public TileBase Tile;
        public List<Vector3Int> Positions;

        public TileSet(TileBase tile, List<Vector3Int> positions)
        {
            Tile = tile;
            Positions = positions;
        }
    }
}