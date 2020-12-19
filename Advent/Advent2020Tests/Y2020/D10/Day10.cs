using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventLibrary;

namespace Advent2020Tests.Days.D10
{
    [TestClass]
    public class Day10
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D10/Data.txt");
        }

        public int[] GetData()
        {
            return GetLines().Select(l =>
            {
                return int.Parse(l);
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(int[] lines)
        {
            lines = lines.Concat(new []{0, 3 + lines.Max(i => i)}).OrderBy(i => i).ToArray();
            int d1 = 0, d3 = 0;
            for (int i = 0; i < lines.Length - 1; i++)
            {
                int diff = lines[i + 1] - lines[i];
                if (diff == 1) d1++;
                if (diff == 3) d3++;
            }
            return d1 * d3;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(int[] lines)
        {
            int device = 3 + lines.Max(i => i);
            lines = lines.Concat(new[] { 0, device}).OrderBy(i => i).ToArray();
            var nums = new HashSet<int>(lines);
            long[] ways = new long[device + 1];
            ways[0] = 1;
            for (int i = 1; i < ways.Length; i++)
            {
                if (nums.Contains(i))
                {
                    ways[i] += ways[i - 1];
                    if (i >= 2) ways[i] += ways[i - 2];
                    if (i >= 3) ways[i] += ways[i - 3];
                }
            }

            return ways.Last();
        }
    }
}
