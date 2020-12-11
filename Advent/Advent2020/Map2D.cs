using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Advent2020
{
    public class Map2D<T>
    {
        private readonly T[][] matrix;
        private readonly T defaultValue;

        public T Get(int y, int x)
        {
            if (!IsInside(y, x)) return defaultValue;
            return matrix[y][x];
        }

        public bool IsInside(int y, int x) => x >= 0 && y >= 0 && y < matrix.Length && x < matrix[y].Length;

        public Point2D<T> this[int y, int x] => new Point2D<T>(this, y, x);

        public int Width => matrix[0].Length;
        public int Height => matrix.Length;

        public Map2D(T[][] matrix, T defaultValue = default)
        {
            this.matrix = matrix;
            this.defaultValue = defaultValue;
            if (matrix.Any(r => r.Any(c => Equals(c, defaultValue))))
                throw new ArgumentException("Matrix has a default value");
        }

        public Map2D(IEnumerable<IEnumerable<T>> matrix, T defaultValue = default)
            : this(matrix.Select(r => r.ToArray()).ToArray(), defaultValue)
        {
        }

        public T[][] NewMatrix()
        {
            T[][] newMatrix = new T[Height][];
            for (int y = 0; y < Height; y++)
            {
                newMatrix[y] = new T[Width];
                for (int x = 0; x < Width; x++)
                {
                    newMatrix[y][x] = defaultValue;
                }
            }

            return newMatrix;
        }

        public IEnumerable<Point2D<T>> GetAllPoints()
        {
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return this[y, x];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var row in matrix)
            {
                if (!first) sb.Append('\n');
                first = false;
                foreach (var item in row)
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }
    }

    public static class MapFactories
    {
        public static Map2D<char> Character(string[] lines, char defaultValue = '~')
        {
            return new Map2D<char>(lines, defaultValue);
        }
    }

    public class Point2D<T>
    {
        private readonly Map2D<T> map;
        public readonly int X, Y;

        public Point2D(Map2D<T> map, int y, int x)
        {
            this.map = map;
            Y = y;
            X = x;
        }

        public T Value => map.Get(Y, X);
        public bool IsInside => map.IsInside(Y, X);

        public Point2D<T> GetInDirection(Direction direction)
        {
            int[] vector = direction.Vector();
            return map[Y + vector[0], X + vector[1]];
        }
    }

    public enum Direction
    {
        LeftUp, Up, RightUp,
        Left, Right,
        LeftDown, Down, RightDown
    }

    public static class DirectionExtensions
    {
        private static readonly int[][] vectors;
        static DirectionExtensions()
        {
            vectors = new int[Directions2D.All.Length][];
            foreach (Direction d in Directions2D.All)
            {
                vectors[(int) d] = VectorHardCode(d);
            }
        }

        public static int[] Vector(this Direction d) => vectors[(int)d];

        private static int[] VectorHardCode(Direction d)
        {
            switch (d)
            {
                case Direction.LeftUp: return new[] {-1, -1};
                case Direction.Up: return new[] {-1, 0};
                case Direction.RightUp: return new[] {-1, 1};
                case Direction.Left: return new[] {0, -1};
                case Direction.Right: return new[] {0, 1};
                case Direction.LeftDown: return new[] {1, -1};
                case Direction.Down: return new[] {1, 0};
                case Direction.RightDown: return new[] {1, 1};
                default: throw new InvalidEnumArgumentException("unhandled direction: " + d);
            }
        }
    }

    public static class Directions2D
    {
        public static readonly Direction[] Cardinal =
            { Direction.Up, Direction.Left, Direction.Down, Direction.Right };

        public static readonly Direction[] Diagonal =
            { Direction.LeftUp, Direction.LeftDown, Direction.RightUp, Direction.RightDown };

        public static readonly Direction[] All = Cardinal.Concat(Diagonal).ToArray();
    }
}
