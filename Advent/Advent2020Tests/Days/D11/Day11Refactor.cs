using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
            return null;
        }
    }

    public class Room2
    {
        public Map2D<char> map;

        public Room2(Map2D<char> map)
        {
            this.map = map;
        }

        public int CountOccupied() => map.GetAllPoints().Count(p => p.Value == '#');

        public void Tick()
        {
        }

    }
}
