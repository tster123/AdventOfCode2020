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
            if (x < 0 || y < 0 || x >= matrix.Length || y >= matrix[x].Length) return defaultValue;
            return matrix[x][y];
        }

        public Point2D<T> this[int x, int y] => new Point2D<T>(this, x, y);

        public Map2D(T[][] matrix, T defaultValue = default)
        {
            this.matrix = matrix;
            this.defaultValue = defaultValue;
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
    }
}
