using System;
using System.Linq;
using Advent2020;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.LibraryTest
{

    [TestClass]
    public class DimensionMapTest
    {

        [TestMethod]
        public void TestGet()
        {
            var map = new DimensionMap<char>(3, new[]
            {
                new Point<char>(new[] {0, 0, 0}, 'a'),
                new Point<char>(new[] {5, 2, 3}, 'b'),
                new Point<char>(new[] {1, 1, 4}, 'c'),
            }, '.');
            Assert.AreEqual('a', map[new Point(new []{0, 0, 0})]);
            Assert.AreEqual('b', map[new Point(new[] { 5, 2, 3 })]);
            Assert.AreEqual('c', map[new Point(new[] { 1,1,4 })]);
            Assert.AreEqual('.', map[new Point(new[] { 1, 0, 0 })]);
        }

        [TestMethod]
        public void TestNeighbors3D()
        {
            Point p = new Point(new[] {1, 2, 3});
            Point[] neighbors = p.GetNeighbors().ToArray();
            Assert.AreEqual(26, neighbors.Length);
            foreach (var n in neighbors)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public void TestNeighbors4D()
        {
            Point p = new Point(new[] { 1, 2, 3, 4 });
            Point[] neighbors = p.GetNeighbors().ToArray();
            Assert.AreEqual(80, neighbors.Length);
            foreach (var n in neighbors)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public void TestNeighbors3DCardinal()
        {
            Point p = new Point(new[] { 1, 2, 3 });
            Point[] neighbors = p.GetNeighbors(includeDiagonals: false).ToArray();
            Assert.AreEqual(6, neighbors.Length);
            foreach (var n in neighbors)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public void TestGetPointsInRange()
        {
            var map = new DimensionMap<char>(3, new[]
            {
                new Point<char>(new[] {0, 0, 0}, 'a'),
                new Point<char>(new[] {5, 2, 3}, 'a'),
                new Point<char>(new[] {1, 1, 4}, 'a'),
            });
            var points = map.EnumeratePointsInRange().ToArray();
            Assert.AreEqual(6 * 3 * 5, points.Length);
            foreach (var p in points)
            {
                Console.WriteLine(p);
            }
        }

        [TestMethod]
        public void TestGetPointsInRangeWeird()
        {
            var map = new DimensionMap<char>(3, new[]
            {
                new Point<char>(new[] {1, 2, 3}, 'a'),
                new Point<char>(new[] {3, 0, 3}, 'a'),
                new Point<char>(new[] {0, -2, 3}, 'a'),
                new Point<char>(new[] {2, -1, 3}, 'a'),
                new Point<char>(new[] {1, 2, 3}, 'a'),
            });
            var points = map.EnumeratePointsInRange().ToArray();
            Assert.AreEqual(5 * 4 * 1, points.Length);
            foreach (var p in points)
            {
                Console.WriteLine(p);
            }
        }

        [TestMethod]
        public void TestGetPointsInRangeExpand()
        {
            var map = new DimensionMap<char>(3, new[]
            {
                new Point<char>(new[] {0, 0, 0}, 'a'),
                new Point<char>(new[] {2, 1, 3}, 'a'),
                new Point<char>(new[] {1, 1, 1}, 'a'),
            });
            var points = map.EnumeratePointsInRange(expandOutBy:1).ToArray();
            Assert.AreEqual(5 * 4 * 6, points.Length);
            foreach (var p in points)
            {
                Console.WriteLine(p);
            }
        }
    }
}
