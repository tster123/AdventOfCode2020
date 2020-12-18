using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Advent2020
{
    public class Point<TVal>
    {
        protected readonly int[] Coordinates;

        public Point([NotNull] int[] coordinates)
        {
            noValue = true;
            Coordinates = coordinates;
        }

        public Point([NotNull] Point<TVal> p)
        {
            noValue = true;
            Coordinates = p.Coordinates;
        }

        public Point([NotNull] int[] coordinates, DimensionMap<TVal> map)
        {
            _map = map;
            Coordinates = coordinates;
        }

        public Point([NotNull] Point<TVal> p, DimensionMap<TVal> map)
        {
            _map = map;
            Coordinates = p.Coordinates;
        }

        public Point([NotNull] int[] coordinates, TVal val)
        {
            _value = val;
            Coordinates = coordinates;
        }

        public Point([NotNull] Point<TVal> p, TVal val)
        {
            _value = val;
            Coordinates = p.Coordinates;
        }

        private bool noValue;
        private readonly TVal _value;
        private DimensionMap<TVal> _map;

        public TVal Value => noValue ? throw new InvalidOperationException("No Value!") :
            _map == null ? _value : _map[this];

        public void SetMap(DimensionMap<TVal> map)
        {
            _map = map;
            noValue = false;
        }

        public int Dimensions => Coordinates.Length;

        public int this[int d] => Coordinates[d];

        public virtual Point<TVal> Wrap([NotNull] Range[] ranges)
        {
            if (ranges.Length != Dimensions)
                throw new ArgumentException("Cannot wrap different dimension");
            int[] newCoords = new int[Dimensions];
            for (int d = 0; d <= Dimensions; d++)
            {
                newCoords[d] = Coordinates[d] % ranges[d].Max;
            }

            return new Point<TVal>(newCoords);
        }

        protected virtual bool InternalEquals(Point<TVal> other)
        {
            return Coordinates.SequenceEqual(other.Coordinates);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return InternalEquals((Point<TVal>)obj);
        }

        public override int GetHashCode()
        {
            int hc = Coordinates.Length;
            foreach (var t in Coordinates)
            {
                hc = unchecked(hc * 314159 + t);
            }
            return hc;
        }

        private string str;
        public override string ToString()
        {
            str = str ?? BuildToString();
            return str;
        }

        protected virtual string BuildToString() => "(" + string.Join(",", Coordinates) + ")";

        public IEnumerable<Point<TVal>> GetNeighbors(bool includeDiagonals = true)
        {
            foreach (int[] delta in GetDirectionVectors(includeDiagonals))
            {
                yield return ApplyVector(delta);
            }
        }

        public virtual Point<TVal> ApplyVector(int[] vector)
        {
            int[] c = new int[Dimensions];
            for (int i = 0; i < vector.Length; i++) c[i] = Coordinates[i] + vector[i];
            return new Point<TVal>(c);
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
                    int[] c = Coordinates.ToArray();
                    c[d]--;
                    yield return c;
                    c = Coordinates.ToArray();
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

    public class Point2D<TVal> : Point<TVal>
    {
        public Point2D(int row, int col) : base(new []{row, col})
        {
        }

        public Point2D(int row, int col, TVal val) : base(new[] { row, col }, val)
        {
        }

        public Point2D(int[] coordinates) : base(coordinates)
        {
        }

        public Point2D(int[] coordinates, TVal val) : base(coordinates, val)
        {
        }

        public Point2D([NotNull] Point<TVal> p) : base(p)
        {
        }

        public Point2D([NotNull] Point<TVal> p, TVal val) : base(p, val)
        {
        }

        protected override bool InternalEquals(Point<TVal> other)
        {
            return Coordinates[0] == other[0] && Coordinates[1] == other[1];
        }

        public override int GetHashCode()
        {
            int hc = Coordinates.Length;
            hc = unchecked(hc * 314159 + Coordinates[0]);
            hc = unchecked(hc * 314159 + Coordinates[1]);
            return hc;
        }

        protected override string BuildToString() => $"({Coordinates[0]},{Coordinates[1]})";

        public override Point<TVal> ApplyVector(int[] vector)
        {
            return new Point2D<TVal>(Coordinates[0] + vector[0], Coordinates[1] + vector[1]);
        }
    }
}