using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Days.D5
{
    [TestClass]
    public class Day5
    {
        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(ToId("BBFBFBFLLL"));
        }

        [TestMethod]
        public void Problem2()
        {
            string[] lines = File.ReadAllLines("./Days/D5/Data.txt");
            var ints = lines.Select(s => ToId(s)).OrderBy(i => i).ToList();
            for (int i = 1; i < ints.Count - 2; i++)
            {
                if (ints[i] + 2 == ints[i+1] && ints[i - 1] == ints[i] - 1)
                {
                    Console.WriteLine(ints[i] + 1);
                }
            }
        }

        public int ToId(string str)
        {
            return Convert.ToInt32(str.Substring(0, 7).Replace("F", "0").Replace("B", "1"), 2) * 8 +
                Convert.ToInt32(str.Substring(7).Replace("L", "0").Replace("R", "1"), 2);
        }
    }
}
