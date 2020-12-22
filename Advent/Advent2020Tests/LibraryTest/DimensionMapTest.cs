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
            Assert.AreEqual('a', map[new Point<char>(new []{0, 0, 0})]);
            Assert.AreEqual('b', map[new Point<char>(new[] { 5, 2, 3 })]);
            Assert.AreEqual('c', map[new Point<char>(new[] { 1,1,4 })]);
            Assert.AreEqual('.', map[new Point<char>(new[] { 1, 0, 0 })]);
        }

        [TestMethod]
        public void TestNeighbors3D()
        {
            var p = new Point<char>(new[] {1, 2, 3});
            Point<char>[] neighbors = p.GetNeighbors().ToArray();
            Assert.AreEqual(26, neighbors.Length);
            foreach (var n in neighbors)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public void TestNeighbors4D()
        {
            Point<char> p = new Point<char>(new[] { 1, 2, 3, 4 });
            Point<char>[] neighbors = p.GetNeighbors().ToArray();
            Assert.AreEqual(80, neighbors.Length);
            foreach (var n in neighbors)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public void TestNeighbors3DCardinal()
        {
            Point<char> p = new Point<char>(new[] { 1, 2, 3 });
            Point<char>[] neighbors = p.GetNeighbors(includeDiagonals: false).ToArray();
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

        [TestMethod]
        public void TestTransmutations()
        {
            string[] lines =
            {
                "0123",
                "4567",
                "89ab"
            };
            var map = MapFactories.Character2D(lines);
            Console.WriteLine(map);
            var rotated = Transmuter<char>.RotateCounterClockwise(map);
            Console.WriteLine(rotated);
            Assert.AreEqual("37b\n26a\n159\n048\n", rotated.ToString());

            rotated = Transmuter<char>.RotateClockwise(map);
            Console.WriteLine(rotated);
            Assert.AreEqual("840\n951\na62\nb73\n", rotated.ToString());

            var flipped = Transmuter<char>.FlipAlongVertical(map);
            Console.WriteLine(flipped);
            Assert.AreEqual("3210\n7654\nba98\n", flipped.ToString());

            flipped = Transmuter<char>.FlipAlongHorizontal(map);
            Console.WriteLine(flipped);
            Assert.AreEqual("89ab\n4567\n0123\n", flipped.ToString());
        }
    }
}
