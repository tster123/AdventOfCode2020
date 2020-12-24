using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Advent2020;
using Advent2020Tests.Common;

namespace Advent2020Tests.Y2020.D24
{

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Direction
    {
        E, SW, SE, W, NW, NE
    }
    [TestClass]
    public class Day24 : AdventTest
    {
        //public override string DataFile => "Test.txt";

        public Direction[][] GetData()
        {
            return GetLines().Select(ParseLine).ToArray();
        }

        private Direction[] ParseLine(string s)
        {
            return ParseLineHelper(s).ToArray();
        }

        private IEnumerable<Direction> ParseLineHelper(string s)
        {
            int i = 0;
            while (i < s.Length)
            {
                if (s[i] == 'e') yield return Direction.E;
                else if (s[i] == 'w') yield return Direction.W;
                else if (s[i] == 's')
                {
                    i++;
                    if (s[i] == 'e') yield return Direction.SE;
                    else if (s[i] == 'w') yield return Direction.SW;
                    else throw new Exception("parse bad");
                }
                else if (s[i] == 'n')
                {
                    i++;
                    if (s[i] == 'e') yield return Direction.NE;
                    else if (s[i] == 'w') yield return Direction.NW;
                    else throw new Exception("parse bad");
                }
                else throw new Exception("parse bad");
                i++;
            }
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(488, Problem1(GetData()));
        }

        private object Problem1(Direction[][] lines)
        {
            var set = SetupInitial(lines);

            return set.Values.Count(b => b);
        }

        private static Dictionary<Point2D<bool>, bool> SetupInitial(Direction[][] lines)
        {
            Dictionary<Point2D<bool>, bool> set = new Dictionary<Point2D<bool>, bool>();
            foreach (Direction[] line in lines)
            {
                int x = 0, y = 0;
                foreach (var dir in line)
                {
                    if (dir == Direction.E) x += 2;
                    else if (dir == Direction.W) x -= 2;
                    else if (dir == Direction.SE)
                    {
                        x += 1;
                        y -= 1;
                    }
                    else if (dir == Direction.SW)
                    {
                        x -= 1;
                        y -= 1;
                    }
                    else if (dir == Direction.NE)
                    {
                        x += 1;
                        y += 1;
                    }
                    else if (dir == Direction.NW)
                    {
                        x -= 1;
                        y += 1;
                    }
                }

                var p = new Point2D<bool>(x, y);
                if (set.TryGetValue(p, out bool val)) set[p] = !val;
                else set[p] = true;
            }

            return set;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(4118, Problem2(GetData()));
        }

        private object Problem2(Direction[][] lines)
        {
            Dictionary<Point2D<bool>, bool> set = SetupInitial(lines);
            for (int i = 0; i < 100; i++)
            {
                set = GetNextDay(set);
            }
            return set.Values.Count(b => b);
        }

        private readonly int[][] vectors = new[]
        {
            new[] {-2, 0},
            new[] {-1, -1},
            new[] {-1, 1},
            new[] {2, 0},
            new[] {1, 1},
            new[] {1, -1}
        };

        private Dictionary<Point2D<bool>, bool> GetNextDay(Dictionary<Point2D<bool>, bool> state)
        {
            int minX = state.Keys.Select(p => p.Vector[0]).Min();
            int maxX = state.Keys.Select(p => p.Vector[0]).Max();
            int minY = state.Keys.Select(p => p.Vector[1]).Min();
            int maxY = state.Keys.Select(p => p.Vector[1]).Max();

            var nextState = new Dictionary<Point2D<bool>, bool>();

            for (int x = minX - 2; x < maxX + 2; x++)
            for (int y = minY - 2; y < maxY + 2; y++)
            {
                if (Math.Abs(y % 2) == 0 && Math.Abs(x % 2) != 0) continue;
                if (Math.Abs(y % 2) == 1 && Math.Abs(x % 2) != 1) continue;
                int count = 0;
                foreach (var v in vectors)
                {
                    var p = new Point2D<bool>(x + v[0], y + v[1]);
                    if (state.TryGetValue(p, out bool value) && value)
                    {
                        count++;
                    }
                }

                var thisP = new Point2D<bool>(x, y);
                if (state.TryGetValue(thisP, out bool wasBlack) && wasBlack)
                {
                    if (count != 0 && count <= 2)  nextState[thisP] = true;
                }
                else
                {
                    if (count == 2) nextState[thisP] = true;
                }
            }

            return nextState;
        }
    }
}
