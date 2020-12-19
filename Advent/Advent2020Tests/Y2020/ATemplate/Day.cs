using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using AdventLibrary;

namespace Advent2020Tests.Days.ATemplate
{
    [TestClass]
    public class Day
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D/Data.txt");
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
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(object[] lines)
        {
            return null;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(object[] lines)
        {
            return null;
        }
    }
}
