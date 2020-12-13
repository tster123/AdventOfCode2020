using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventLibrary;

namespace Advent2020Tests.Days.D13
{
    [TestClass]
    public class Day13
    {
        public const int EarliestTime = 1001171;
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D13/Data.txt");
        }

        public object[] GetData()
        {
            return GetLines().Select(l =>
            {
                return (object) null;
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetLines().First()));
        }

        private object Problem1(string line)
        {
            int[] busIds = line.Split(",").Where(s => s != "x").Select(s => int.Parse(s)).ToArray();
            int earliest = int.MaxValue;
            int earliestId = 0;
            foreach (int id in busIds)
            {
                int times = EarliestTime / id;
                int prev = times * id;
                int next = prev + id;
                if (earliest > next)
                {
                    earliest = next;
                    earliestId = id;
                }
            }

            return (earliest - EarliestTime) * earliestId;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetLines().First()));
            //Console.WriteLine(Problem2("17,x,13,19"));
            //Console.WriteLine(Problem2("1789,37,47,1889"));
        }

        private object Problem2(string line)
        {
            string[] parts = line.Split(",");
            var busses = new List<Bus>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "x") busses.Add(new Bus(int.Parse(parts[i]), i));
            }

            Bus largestBus = busses.OrderByDescending(b => b.Id).First();
            long candidate = largestBus.Id - largestBus.Offset;
            Bus[] unaccounted = busses.Where(b => b != largestBus).ToArray();
            long toAdd = largestBus.Id;
            while (unaccounted.Length > 0)
            {
                foreach (Bus b in unaccounted)
                {
                    if ((candidate + b.Offset) % b.Id == 0)
                    {
                        toAdd *= b.Id;
                        unaccounted = unaccounted.Where(b2 => b2 != b).ToArray();
                    }
                }

                candidate += toAdd;
            }

            return candidate - toAdd;
        }
    }
    public class Bus
    {
        public int Id;
        public int Offset;

        public Bus(int id, int offset)
        {
            Id = id;
            Offset = offset;
        }
    }
}
