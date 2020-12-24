using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2020.D24
{

    public enum Direction
    {
        E, SW, SE, W, NW, NE
    }
    [TestClass]
    public class Day24 : AdventTest
    {
        public virtual string DataFile => "Test.txt";

        public Direction[][] GetData()
        {
            return GetLines().Select(l =>
            {
                return ParseLine(l);
            }).ToArray();
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
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Direction[][] lines)
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

                var p = new Point2D<bool>(y, x);
                if (set.TryGetValue(p, out bool val)) set[p] = !val;
                else set[p] = true;
            }

            return set.Values.Count(b => b);
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(object[] lines)
        {
            return null;
        }
    }
}
