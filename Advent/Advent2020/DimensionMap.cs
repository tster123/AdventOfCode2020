using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Advent2020
{
    public class DimensionMap<TVal>
    {
        private readonly Dictionary<Point, TVal> values = new Dictionary<Point, TVal>();
        public readonly int Dimensions;
        private readonly Range[] ranges;
        private readonly bool isInfinite;
        private readonly bool wraps;
        private readonly TVal defaultValue;

        public DimensionMap(int dimensions, [NotNull] IEnumerable<Point<TVal>> points, TVal defaultValue = default, bool isInfinite = true, bool wraps = false)
        {
            Dimensions = dimensions;
            ranges = new Range[dimensions];
            this.isInfinite = isInfinite;
            this.wraps = wraps;
            this.defaultValue = defaultValue;

            if (isInfinite && wraps) throw new ArgumentException("Cannot wrap and be infinite");
            
            foreach (Point<TVal> p in points)
            {
                if (p.Dimensions != Dimensions) throw new ArgumentException("Unequal dimensions");
                this[p] = p.Value;
            }
        }

        public Range GetRange(int dimension) => ranges[dimension];

        public TVal this[[NotNull]Point p]
        {
            get
            {
                if (wraps)
                {
                    p = p.Wrap(ranges);
                }
                if (values.TryGetValue(p, out var v)) return v;
                return defaultValue;
            }
            private set
            {
                if (p.Dimensions != Dimensions)
                    throw new ArgumentException(
                        $"map dimensions ({Dimensions}) are different than point dimensions ({p.Dimensions})");
                for (int d = 0; d < ranges.Length; d++)
                {
                    if (wraps)
                    {
                        if (p[d] < 0) throw new ArgumentException("cannot handle wrapping with coordinates < 0: " + p);
                    }
                    ranges[d] = ranges[d]?.ExtendTo(p[d]) ?? new Range(p[d], p[d]);
                }
                values[p] = value;
            }
        }

        public IEnumerable<Point<TVal>> GetPointsWithValues()
        {
            foreach (var p in values.Keys) yield return new Point<TVal>(p, this[p]);
        }

        public IEnumerable<Point> Points => values.Keys;

        public IEnumerable<Point> EnumeratePointsInRange(int expandOutBy = 0)
        {
            int[] deltas = new int[Dimensions];
            Range[] newRanges = ranges;
            if (expandOutBy > 0)
            {
                for (int i = 0; i < newRanges.Length; i++)
                {
                    newRanges[i] = new Range(newRanges[i].Min - expandOutBy, newRanges[i].Max + expandOutBy);
                }
            }
            for (int i = 0; i < deltas.Length; i++) deltas[i] = newRanges[i].Min;
            do
            {
                yield return new Point(deltas.ToArray());
            } while (Point.BumpDeltas(deltas, newRanges));
        }
    }

    public class Point
    {
        private readonly int[] coordinates;

        public Point([NotNull] int[] coordinates)
        {
            this.coordinates = coordinates;
        }

        public Point([NotNull] Point p)
        {
            this.coordinates = p.coordinates;
        }

        public int Dimensions => coordinates.Length;

        public int this[int d] => coordinates[d];

        public Point Wrap([NotNull] Range[] ranges)
        {
            if (ranges.Length != Dimensions)
                throw new ArgumentException("Cannot wrap different dimension");
            int[] newCoords = new int[Dimensions];
            for (int d = 0; d <= Dimensions; d++)
            {
                newCoords[d] = coordinates[d] % ranges[d].Max;
            }

            return new Point(newCoords);
        }

        protected bool Equals(Point other)
        {
            return coordinates.SequenceEqual(other.coordinates);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            int hc = coordinates.Length;
            foreach (var t in coordinates)
            {
                hc = unchecked(hc * 314159 + t);
            }
            return hc;
        }

        private string str;
        public override string ToString()
        {
            str = str ?? "(" + string.Join(",", coordinates) + ")";
            return str;
        }

        public IEnumerable<Point> GetNeighbors(bool includeDiagonals = true)
        {
            if (includeDiagonals)
            {
                int[] deltas = new int[Dimensions];
                for (int i = 0; i < deltas.Length; i++) deltas[i] = -1;
                do
                {
                    if (deltas.Any(a => a != 0))
                    {
                        int[] c = new int[Dimensions];
                        for (int i = 0; i < deltas.Length; i++) c[i] = coordinates[i] + deltas[i];
                        yield return new Point(c);
                    }
                } while (BumpDeltas(deltas, null));
            }
            else
            {
                for (int d = 0; d < Dimensions; d++)
                {
                    int[] c = coordinates.ToArray();
                    c[d]--;
                    yield return new Point(c);
                    c = coordinates.ToArray();
                    c[d]++;
                    yield return new Point(c);
                }
            }
        }

        internal static bool BumpDeltas(int[] deltas, Range[] ranges)
        {
            int i = deltas.Length - 1;
            while (i >= 0 && deltas[i] == (ranges?[i].Max ?? 1))
            {
                i--;
            }

            if (i == -1) return false;
            deltas[i]++;
            i++;
            while (i < deltas.Length)
            {
                deltas[i] = ranges?[i].Min ?? -1;
                i++;
            }
            return true;
        }
    }

    public class Point<TVal> : Point
    {
        public readonly TVal Value;

        public Point([NotNull] int[] coordinates, TVal value = default) : base(coordinates)
        {
            Value = value;
        }

        public Point(Point p, TVal value = default) : base(p)
        {
            Value = value;
        }

        public Point(Point<TVal> p) : base(p)
        {
            Value = p.Value;
        }
    }

    public static class MapFactories
    {
        public static DimensionMap<char> Character2D(string[] lines, char defaultValue = '~', bool isInfinite = true, bool wraps = false)
        {
            List<Point<char>> points = new List<Point<char>>();
            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    points.Add(new Point<char>(new[] {row, col}, lines[row][col]));
                }
            }

            return new DimensionMap<char>(2, points, defaultValue, isInfinite, wraps);
        }
    }
}
