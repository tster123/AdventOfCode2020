using System.Linq;
using Advent2020;
using Advent2020Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2022.D4
{
    [TestClass]
    public class Day4 : AdventTest
    {
        public class Assignments
        {
            public Range a, b;

            public Assignments(int a1, int a2, int b1, int b2)
            {
                a = new Range(a1, a2);
                b = new Range(b1, b2);
            }
        }
        public Assignments[] GetData()
        {
            return GetLines().Select(l =>
            {
                string[] parts = l.Split(',');
                return new Assignments(
                    int.Parse(parts[0].Split('-')[0]),
                    int.Parse(parts[0].Split('-')[1]),
                    int.Parse(parts[1].Split('-')[0]),
                    int.Parse(parts[1].Split('-')[1])
                );
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Assignments[] lines)
        {
            int sum = 0;
            foreach (var a in lines)
            {
                if (a.a.FullyContains(a.b) || a.b.FullyContains(a.a)) sum++;
            }

            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Assignments[] lines)
        {
            int sum = 0;
            foreach (var a in lines)
            {
                if (a.a.Overlaps(a.b)) sum++;
            }

            return sum;
        }
    }
}
