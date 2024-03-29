﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Advent2020
{
    public class DimensionMap<TVal>
    {
        private readonly Dictionary<Point<TVal>, TVal> values = new Dictionary<Point<TVal>, TVal>();
        public readonly int Dimensions;
        private readonly Range[] ranges;
        private readonly bool isInfinite;
        private readonly bool wraps;
        private readonly TVal defaultValue;

        public DimensionMap(int dimensions, [NotNull] IEnumerable<Point<TVal>> points, TVal defaultValue = default,
            bool isInfinite = true, bool wraps = false)
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
            if (Dimensions == 2) Formatter = new MapFormatter2D<TVal>();
        }

        public IMapFormatter<TVal> Formatter { get; set; }
        public override string ToString() => Formatter.FormatterMap(this);
        public void Print() => Console.WriteLine(this);

        public DimensionMap<TVal> Next([NotNull] IEnumerable<Point<TVal>> points)
        {
            return new DimensionMap<TVal>(Dimensions, points, defaultValue, isInfinite, wraps);
        }

        public Range GetRange(int dimension) => ranges[dimension];

        public TVal this[[NotNull] Point<TVal> p]
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

        public bool IsInside(Point<TVal> p)
        {
            for (int d = 0; d < Dimensions; d++)
            {
                if (ranges[d].Min > p[d] || ranges[d].Max < p[d]) return false;
            }

            return true;
        }

        public IEnumerable<Point<TVal>> Points => values.Keys;

        public IEnumerable<Point<TVal>> EnumeratePointsInRange(int expandOutBy = 0)
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
                yield return new Point<TVal>(deltas.ToArray());
            } while (Point<TVal>.BumpDeltas(deltas, newRanges));
        }
    }

    public class Transmuter<TVal>
    {
        public static DimensionMap<TVal> RotateCounterClockwise(DimensionMap<TVal> map)
        {
            return Transform2D(map, p => new[] { p[1], map.GetRange(0).Max - p[0] });
        }

        public static DimensionMap<TVal> RotateClockwise(DimensionMap<TVal> map)
        {
            return Transform2D(map, p => new[] { map.GetRange(1).Max - p[1], p[0] });
        }

        public static DimensionMap<TVal> FlipAlongVertical(DimensionMap<TVal> map)
        {
            return Transform2D(map, p => new[] { map.GetRange(0).Max - p[0], p[1]});
        }

        public static DimensionMap<TVal> FlipAlongHorizontal(DimensionMap<TVal> map)
        {
            return Transform2D(map, p => new[] {p[0], map.GetRange(1).Max - p[1]});
        }

        private static DimensionMap<TVal> Transform2D(DimensionMap<TVal> map, Func<Point<TVal>, int[]> transformer)
        {
            if (map.Dimensions != 2) throw new ArgumentException("only handles 2D maps");
            var points = new List<Point<TVal>>();
            foreach (var p in map.Points)
            {
                points.Add(new Point<TVal>(transformer(p), p.Value));
            }

            return new DimensionMap<TVal>(2, points);
        }
    }

    public interface IMapFormatter<TVal>
    {
        string FormatterMap(DimensionMap<TVal> map);
    }

    public class MapFormatter2D<TVal> : IMapFormatter<TVal>
    {
        public string FormatterMap(DimensionMap<TVal> map)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = map.GetRange(1).Min; row <= map.GetRange(1).Max; row++)
            {
                for (int col = map.GetRange(0).Min; col <= map.GetRange(0).Max; col++)
                {
                    sb.Append(map[new Point<TVal>(new[]{col, row})]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
    }

    public static class MapFactories
    {
        public static DimensionMap<char> Character2D(IEnumerable<string> lines, char defaultValue = '~', bool isInfinite = true,
            bool wraps = false)
        {
            List<Point<char>> points = new List<Point<char>>();
            int row = 0;
            foreach (string line in lines)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    points.Add(new Point2D<char>(new[] {col, row}, line[col]));
                }

                row++;
            }

            return new DimensionMap<char>(2, points, defaultValue, isInfinite, wraps);
        }

        public static DimensionMap<TFinal> Flexible2D<TFinal>(
            IEnumerable<string> lines, 
            Func<string, IEnumerable<string>> lineSplitter,
            Func<string, TFinal> parser,
            TFinal defaultValue,
            bool isInfinite = true,
            bool wraps = false)
        {
            List<Point<TFinal>> points = new List<Point<TFinal>>();
            int row = 0;
            foreach (string line in lines)
            {
                var parts = lineSplitter(line);
                int col = 0;
                foreach (string part in parts)
                {
                    points.Add(new Point2D<TFinal>(new[] {col, row}, parser(part)));
                    col++;
                }

                row++;
            }

            return new DimensionMap<TFinal>(2, points, defaultValue, isInfinite, wraps);
        }
    }
}