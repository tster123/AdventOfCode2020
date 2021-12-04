using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventLibrary;

namespace Advent2020Tests.Y2021.D01
{
    [TestClass]
    public class Day01
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2021/D01/Data.txt");
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
            int ret = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i - 1] < lines[i]) ret++;
            }
            return ret;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(int[] lines)
        {
            int ret = 0;
            for (int i = 3; i < lines.Length; i++)
            {
                if (lines[i - 3] < lines[i]) ret++;
            }
            return ret;
        }
    }
}
