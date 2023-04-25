using System;

namespace Code.DungeonGeneration
{
    public struct RoomIndex : IEquatable<RoomIndex>
    {
        public readonly int X;
        public readonly int Y;

        public RoomIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
        
        public static bool operator !=(RoomIndex obj1, RoomIndex obj2)
        {
            return !(obj1.Equals(obj2));
        }

        public static bool operator ==(RoomIndex obj1, RoomIndex obj2)
        {
            return (obj1.Equals(obj2));
        }

        public bool Equals(RoomIndex other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is RoomIndex other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public static class Extensions
    {
        public static RoomIndex North(this RoomIndex index)
        {
            return new RoomIndex(index.X, index.Y + 1);
        }
        public static RoomIndex South(this RoomIndex index)
        {
            return new RoomIndex(index.X, index.Y - 1);
        }
        public static RoomIndex East(this RoomIndex index)
        {
            return new RoomIndex(index.X + 1, index.Y);
        }
        public static RoomIndex West(this RoomIndex index)
        {
            return new RoomIndex(index.X - 1, index.Y);
        }
        
    }
}