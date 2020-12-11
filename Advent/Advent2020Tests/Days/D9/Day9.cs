using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using AdventLibrary;

namespace Advent2020Tests.Days.D9
{
    [TestClass]
    public class Day9
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D9/Data.txt");
        }

        public long[] GetData()
        {
            return GetLines().Select(l =>
            {
                return long.Parse(l);
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(long[] lines)
        {

            for (int i = 25; i < lines.Length; i++)
            {
                if (!IsValid(i, lines)) return lines[i];
            }
            return null;
        }

        public bool IsValid(int i, long[] lines)
        {
            for (int x = i - 25; x < i - 1; x++)
            {
                for (int y = x + 1; y < i; y++)
                {
                    if (lines[x] == lines[y]) continue;
                    if (lines[x] + lines[y] == lines[i]) return true;
                }
            }

            return false;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(long[] lines)
        {
            long key = 14360655;
            for (int x = 0; x < lines.Length - 1; x++)
            {
                long sum = lines[x];
                long min = lines[x];
                long max = lines[x];
                for (int y = x + 1; y < lines.Length; y++)
                {
                    sum += lines[y];
                    min = Math.Min(min, lines[y]);
                    max = Math.Max(max, lines[y]);
                    if (sum == key) return min + max;
                    if (sum > key) break;
                }
            }

            return "laknsdklnas";
        }
    }
}
