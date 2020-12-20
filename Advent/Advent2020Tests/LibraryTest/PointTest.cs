using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2020;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.LibraryTest
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void TestEquality()
        {
            Point<int> a = null,
                b = null,
                c = new Point<int>(new[] {1}),
                d = new Point<int>(new[] {1, 2}, 3),
                e = new Point<int>(new[] {1, 2}, 3),
                f = d,
                g = new Point<int>(new[] {1, 2}, 4);

            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
            Assert.IsTrue(d == f);
            Assert.IsTrue(f == d);
            Assert.IsFalse(d != f);
            Assert.IsTrue(d == e);
            Assert.IsTrue(e == g);
        }
    }
}
