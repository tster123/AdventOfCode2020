using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AdventLibrary;

namespace Advent2020Tests.Days.D15
{
    [TestClass]
    public class Day15
    {


        public int[] GetData()
        {
            return new[] {13, 16, 0, 12, 15, 1};
            //return new[] { 1,2, 3 };
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(int[] data)
        {
            return LastNum(data, 2020);
        }

        private static object LastNum(int[] data, int iterations)
        {
            Dictionary<int, List<int>> speaks = new Dictionary<int, List<int>>();
            int time = 1;
            int lastAge = 0;
            foreach (int d in data)
            {
                if (speaks.ContainsKey(d))
                {
                    speaks[d].Add(time);
                }
                else
                {
                    speaks[d] = new List<int> {time};
                }

                time++;
            }

            int lastNum = data.Last();

            while (time <= iterations)
            {
                int next;
                if (speaks[lastNum].Count == 1)
                {
                    next = 0;
                }
                else
                {
                    next = speaks[lastNum].Last() - speaks[lastNum][speaks[lastNum].Count - 2];
                }

                if (speaks.ContainsKey(next))
                {
                    speaks[next].Add(time);
                }
                else
                {
                    speaks[next] = new List<int> {time};
                }

                lastNum = next;
                time++;
            }

            return lastNum;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(int[] data)
        {
            return LastNum(data, 30000000);
        }
    }

}
