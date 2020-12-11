using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020
{
    public class Map2D<T>
    {
        private readonly T[][] matrix;
        private readonly T defaultValue;

        public T Get(int x, int y)
        {
            if (!IsInside(x, y)) return defaultValue;
            return matrix[x][y];
        }

        public bool IsInside(int x, int y) => x >= 0 && y >= 0 && x < matrix.Length && y < matrix[x].Length;

        public Point2D<T> this[int x, int y] => new Point2D<T>(this, x, y);

        public Map2D(T[][] matrix, T defaultValue = default)
        {
            this.matrix = matrix;
            this.defaultValue = defaultValue;
        }

        public Map2D(IEnumerable<IEnumerable<T>> matrix, T defaultValue = default)
        {
            this.matrix = matrix.Select(r => r.ToArray()).ToArray();
            this.defaultValue = defaultValue;
        }

        public IEnumerable<Point2D<T>> GetAllPoints()
        {
            for (int x = 0; x < matrix.Length; x++)
            for (int y = 0; y < matrix[x].Length; y++)
                yield return this[x, y];
        }

        public static readonly Direction[] CardinalDirections =
            new[] {Direction.Up, Direction.Left, Direction.Down, Direction.Right};

        public static readonly Direction[] DiagonalDirections =
            new[] { Direction.LeftUp, Direction.LeftDown, Direction.RightUp, Direction.RightDown };

        public static readonly Direction[] AllDirections = CardinalDirections.Concat(DiagonalDirections).ToArray();
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

        public Point2D(Map2D<T> map, int x, int y)
        {
            this.map = map;
            X = x;
            Y = y;
        }

        public T Value => map.Get(X, Y);
        public bool IsInside => map.IsInside(X, Y);

        public Point2D<T> GetInDirection(Direction direction)
        {
            int x = X;
            int y = Y;
            if (direction.ToString().Contains("Left")) x--;
            if (direction.ToString().Contains("Right")) x++;
            if (direction.ToString().Contains("Up")) y++;
            if (direction.ToString().Contains("Down")) y--;
            return map[x, y];
        }
    }

    public enum Direction
    {
        LeftUp, Up, RightUp,
        Left, Right,
        LeftDown, Down, RightDown
    }
}
