using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2021.D02
{
    public class Command
    {
        public int Horizontal;
        public int Depth;
    }

    [TestClass]
    public class Day02 : AdventTest
    {

        public Command[] GetData()
        {
            return GetLines().Select(l =>
            {
                var a = l.Split(' ')[0];
                int v = int.Parse(l.Split(' ')[1]);
                return new Command
                {
                    Horizontal = a == "forward" ? v : 0,
                    Depth = a == "forward" ? 0 : a == "up" ? -v : v
                };
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Command[] lines)
        {
            int d = 0, h = 0;
            foreach (var c in lines)
            {
                d += c.Depth;
                h += c.Horizontal;
            }
            return d * h;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Command[] lines)
        {
            int d = 0, h = 0, a = 0;
            foreach (var c in lines)
            {
                a += c.Depth;
                h += c.Horizontal;
                d += c.Horizontal * a;
            }
            return d * h;
        }
    }
}
