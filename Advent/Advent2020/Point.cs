using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Advent2020
{
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
            foreach (int[] delta in GetDirectionVectors(includeDiagonals))
            {
                yield return ApplyVector(delta);
            }
        }

        public Point ApplyVector(int[] vector)
        {
            int[] c = new int[Dimensions];
            for (int i = 0; i < vector.Length; i++) c[i] = coordinates[i] + vector[i];
            return new Point(c);
        }

        private readonly int[][] vectors2DAll = new[]
        {
            new[] {-1, -1},
            new[] {-1, 0},
            new[] {-1, 1},
            new[] {0, -1},
            new[] {0, 1},
            new[] {1, -1},
            new[] {1, 0},
            new[] {1, 1}
        };

        private readonly int[][] vectors2DCardinal = new[]
        {
            new[] {-1, 0},
            new[] {0, -1},
            new[] {0, 1},
            new[] {1, 0}
        };

        public IEnumerable<int[]> GetDirectionVectors(bool includeDiagonals = true)
        {
            if (Dimensions == 2) return includeDiagonals ? vectors2DAll : vectors2DCardinal;
            return BuildArbitraryDirectionVectors(includeDiagonals);
        }

        private IEnumerable<int[]> BuildArbitraryDirectionVectors(bool includeDiagonals)
        {
            if (includeDiagonals)
            {
                int[] deltas = new int[Dimensions];
                for (int i = 0; i < deltas.Length; i++) deltas[i] = -1;
                do
                {
                    if (deltas.Any(a => a != 0))
                    {
                        yield return deltas.ToArray();
                    }
                } while (BumpDeltas(deltas, null));
            }
            else
            {
                for (int d = 0; d < Dimensions; d++)
                {
                    int[] c = coordinates.ToArray();
                    c[d]--;
                    yield return c;
                    c = coordinates.ToArray();
                    c[d]++;
                    yield return c;
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
}