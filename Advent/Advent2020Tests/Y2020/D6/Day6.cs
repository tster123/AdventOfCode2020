using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using Advent2020;

namespace Advent2020Tests.Y2020.D6
{
    [TestClass]
    public class Day6
    {
        public string[] GetData()
        {
            return File.ReadAllLines("./Y2020/D6/Data.txt");
        }

        public bool[] GetAnswers(string line)
        {
            bool[] ret = new bool[26];
            for (char a = 'a'; a <= 'z'; a++)
            {
                if (line.Contains(a + ""))
                {
                    ret[a - 'a'] = true;
                }
            }

            return ret;
        }

        public bool[] Consolidate(bool[][] a)
        {
            var ret = new bool[26];
            foreach (var b in a)
            {
                for (int i = 0; i < ret.Length; i++)
                {
                    if (b[i]) ret[i] = true;
                }
            }

            return ret;
        }

        public bool[] Consolidate2(bool[][] a)
        {
            var ret = new bool[26];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = true;

            foreach (var b in a)
            {
                for (int i = 0; i < ret.Length; i++)
                {
                    if (!b[i]) ret[i] = false;
                }
            }

            return ret;
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(string[] lines)
        {
            var groups = LineHelper.Groupify(lines);
            int count = 0;
            foreach (var g in groups)
            {
                count += Consolidate(g.Select(line => GetAnswers(line)).ToArray()).Count(b => b);
            }
            return count;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(string[] lines)
        {
            var groups = LineHelper.Groupify(lines);
            int count = 0;
            foreach (var g in groups)
            {
                count += Consolidate2(g.Select(line => GetAnswers(line)).ToArray()).Count(b => b);
            }
            return count;
        }
    }
}
