using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Advent2020Tests.Days.ATemplate
{
    [TestClass]
    public class Day
    {
        public string[] GetData()
        {
            return File.ReadAllLines("./Days/D5/Data.txt");
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(string[] lines)
        {
            return null;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(string[] lines)
        {
            return null;
        }
    }
}
