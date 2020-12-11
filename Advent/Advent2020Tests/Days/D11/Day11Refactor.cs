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
            return new Room2(MapFactories.Character(GetLines()));
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(Room2 room)
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
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(Room2 room)
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
        private Map2D<char> map;

        public Room2(Map2D<char> map)
        {
            this.map = map;
        }

        public int CountOccupied() => map.GetAllPoints().Count(p => p.Value == '#');

        public override string ToString()
        {
            return map.ToString();
        }

        public void Tick()
        {
            char[][] matrix = map.NewMatrix();
            foreach (var point in map.GetAllPoints())
            {
                if (point.Value == '.')
                {
                    matrix[point.Y][point.X] = '.';
                    continue;
                }

                int count = 0;
                foreach (Direction d in Directions2D.All)
                {
                    var p = point.GetInDirection(d);
                    if (p.Value == '#') count++;
                }

                matrix[point.Y][point.X] = count == 0 ? '#' : count >= 4 ? 'L' : point.Value;
            }

            map = new Map2D<char>(matrix);
        }

        public void Tick2()
        {
            char[][] matrix = map.NewMatrix();
            foreach (var point in map.GetAllPoints())
            {
                if (point.Value == '.')
                {
                    matrix[point.Y][point.X] = '.';
                    continue;
                }

                int count = 0;
                foreach (Direction d in Directions2D.All)
                {
                    var p = point;
                    while (true)
                    {
                        p = p.GetInDirection(d);
                        if (p.Value == '#')
                        {
                            count++;
                            break;
                        }
                        if (p.Value == 'L')
                        {
                            break;
                        }
                        if (!p.IsInside) break;
                    }
                    
                }

                matrix[point.Y][point.X] = count == 0 ? '#' : count >= 5 ? 'L' : point.Value;
            }

            map = new Map2D<char>(matrix);
        }

    }
}
