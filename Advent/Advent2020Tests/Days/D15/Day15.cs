using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AdventLibrary;

namespace Advent2020Tests.Days.D15
{
    /*
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
            Dictionary<int, Memory> speaks = new Dictionary<int, Memory>(1000);
            int time = 1;
            foreach (int d in data)
            {
                if (speaks.TryGetValue(d, out var l))
                {
                    l.GetToSpeak(time);
                }
                else
                {
                    speaks[d] = new Memory(time);
                }

                time++;
            }

            int lastNum = data.Last();

            while (time <= iterations)
            {
                var mem = speaks[lastNum];
                lastNum = mem.GetToSpeak(time);
                

                if (speaks.TryGetValue(lastNum, out var l))
                {
                    l.Add(time);
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
            int expected = 2424;
            int actual = (int)Problem2(GetData());
            Assert.AreEqual(expected, actual);
            Console.WriteLine(actual);
        }

        private object Problem2(int[] data)
        {
            return LastNum(data, 30000000);
        }
    }

    public class Memory
    {
        public int Prev, BeforePrev = -1;

        public Memory(int prev)
        {
            Prev = prev;
        }

        public int GetToSpeak(int currentTime)
        {
            if (BeforePrev == -1)
            {
                BeforePrev = Prev;
                Prev = currentTime;
                return 0;
            }
            else
            {
                int ret = currentTime - Prev;
                BeforePrev = Prev;
                Prev = currentTime;
                return ret;
            }
        }
    }
    */
}
