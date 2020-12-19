using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using AdventLibrary;

namespace Advent2020Tests.Days.D17
{

    [TestClass]
    public class Day17
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D17/Data.txt");
        }

        public DimensionMap<char> GetData()
        {
            return MapFactories.Character2D(GetLines(), '.');
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private DimensionMap<char> space;

        private object Problem1(DimensionMap<char> start)
        {
            space = new DimensionMap<char>(3, start.Points
                .Select(p => new Point<char>(new[] { p[0], p[1], 0 }, p.Value)));


            for (int i = 0; i < 6; i++)
            {
                Tick(3);
            }

            return space.Points.Count(p => p.Value == '#');
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(DimensionMap<char> start)
        {
            space = new DimensionMap<char>(4, start.Points.Select(p => new Point<char>(new[] {p[0], p[1], 0, 0}, p.Value)));


            for (int i = 0; i < 6; i++)
            {
                Tick(4);
            }

            return space.Points.Count(p => p.Value == '#');
        }

        private void Tick(int dimension)
        {
            space = new DimensionMap<char>(dimension, NextActive(), '.');
        }

        private IEnumerable<Point<char>> NextActive()
        {
            foreach (var p in space.EnumeratePointsInRange(expandOutBy:1))
            {
                int count = p.GetNeighbors().Count(n => space[n] == '#');
                if (space[p] == '#')
                {
                    if (count == 2 || count == 3) yield return new Point<char>(p, '#');
                }
                else
                {
                    if (count == 3) yield return new Point<char>(p, '#');
                }
            }
        }
    }
}
