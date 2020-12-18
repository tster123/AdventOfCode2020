using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Days.D11
{
    [TestClass]
    public class Day11Refactor
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D11/Data.txt");
        }

        public Room2 GetData()
        {
            return new Room2(MapFactories.Character2D(GetLines()));
        }

        [TestMethod]
        public void Problem1()
        {
            int actual = Problem1(GetData());
            Assert.AreEqual(2344, actual);
            Console.WriteLine(actual);
        }

        private int Problem1(Room2 room)
        {
            HashSet<string> seen = new HashSet<string>();
            while (!seen.Contains(room.ToString()))
            {
                seen.Add(room.ToString());
                room.Tick();
            }

            return room.CountOccupied();
        }

        [TestMethod]
        public void Problem2()
        {
            int actual = Problem2(GetData());
            Assert.AreEqual(2076, actual);
            Console.WriteLine(actual);
        }

        private int Problem2(Room2 room)
        {
            HashSet<string> seen = new HashSet<string>();
            while (!seen.Contains(room.ToString()))
            {
                seen.Add(room.ToString());
                room.Tick2();
            }

            return room.CountOccupied();
        }
    }

    public class Room2
    {
        private DimensionMap<char> map;

        public Room2(DimensionMap<char> map)
        {
            this.map = map;
        }

        public int CountOccupied() => map.GetPointsWithValues().Count(p => p.Value == '#');

        public override string ToString()
        {
            return map.ToString();
        }

        public void Tick()
        {
            var nextPoints = new List<Point<char>>();
            foreach (var point in map.GetPointsWithValues())
            {
                if (point.Value == '.')
                {
                    nextPoints.Add(new Point<char>(point, '.'));
                    continue;
                }

                int count = 0;
                foreach (var p in point.GetNeighbors())
                {
                    if (map[p] == '#') count++;
                }

                char val = count == 0 ? '#' : count >= 4 ? 'L' : point.Value;
                nextPoints.Add(new Point<char>(point, val));
            }

            map = map.Next(nextPoints);
        }

        public void Tick2()
        {
            var nextPoints = new List<Point<char>>();
            foreach (var point in map.GetPointsWithValues())
            {
                if (point.Value == '.')
                {
                    nextPoints.Add(new Point<char>(point, '.'));
                    continue;
                }

                int count = 0;
                foreach (int[] vector in point.GetDirectionVectors())
                {
                    Point p = point;
                    while (true)
                    {
                        p = p.ApplyVector(vector);
                        if (map[p] == '#')
                        {
                            count++;
                            break;
                        }
                        if (map[p] == 'L')
                        {
                            break;
                        }
                        if (!map.IsInside(p)) break;
                    }
                    
                }

                char val = count == 0 ? '#' : count >= 5 ? 'L' : point.Value;
                nextPoints.Add(new Point<char>(new[] { point[0], point[1] }, val));
            }

            map = map.Next(nextPoints);
        }

    }
}
