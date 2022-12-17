using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2021.D04
{
    [TestClass]
    public class Day04 : AdventTest
    {
        public class Space
        {
            public int Value;
            public bool Called;
        }

        public class Input
        {
            public int[] Draws;
            public List<DimensionMap<Space>> Boards;
        }

        public Input GetData()
        {
            string[] lines = GetLines();
            int[] draws = lines[0].Split(',').Select(s => int.Parse(s)).ToArray();
            List<DimensionMap<Space>> boards = lines.Skip(2).Groupify().Select(
                group => MapFactories.Flexible2D(group,
                    lineSplitter: line => line.Split(" ").Where(p => p.Length > 0),
                    parser: part => new Space {Value = int.Parse(part)},
                    defaultValue: null,
                    isInfinite: false)).ToList();
            return new Input
            {
                Boards = boards,
                Draws = draws
            };
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Input input)
        {
            return null;
        }

        [TestMethod]
        public void Problem2()
        {
            //GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(object[] lines)
        {
            return null;
        }
    }
}
