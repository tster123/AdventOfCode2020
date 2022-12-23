using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020Tests.Common;
using AdventLibrary;
using System.Collections;

namespace Advent2020Tests.Y2022.D3
{
    [TestClass]
    public class Day3 : AdventTest
    {
        public class Rucksack
        {
            public Dictionary<char, int> c1 = new Dictionary<char, int>();
            public Dictionary<char, int> c2 = new Dictionary<char, int>();

            public void AddItem(char c, bool c1)
            {
                var comp = c1 ? this.c1 : c2;
                if (!comp.ContainsKey(c))
                {
                    comp[c] = 1;
                }
                else
                {
                    comp[c] += 1;
                }
            }

            public bool Contains(char c) => c1.ContainsKey(c) || c2.ContainsKey(c);
        }

        public Rucksack[] GetData()
        {
            return GetLines().Select(l =>
            {
                var r = new Rucksack();
                for (int i = 0; i < l.Length; i++)
                {
                    r.AddItem(l[i], i < l.Length / 2);
                }

                return r;
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }



        private object Problem1(Rucksack[] sacks)
        {
            int sum = 0;
            foreach (var sack in sacks)
            {
                foreach (char c in sack.c1.Keys)
                {
                    if (sack.c2.ContainsKey(c))
                    {
                        sum += GetValue(c);
                    }
                }
            }

            return sum;
        }

        private int GetValue(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (int)1 + (c - 'a');
            }

            return 27 + (c - 'A');
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Rucksack[] sacks)
        {
            int sum = 0;
            for (int i = 0; i < sacks.Length; i += 3)
            {
                foreach (char c in sacks[i].c1.Keys.Concat(sacks[i].c2.Keys))
                {
                    if (sacks[i+1].Contains(c) && sacks[i+2].Contains(c))
                        sum += GetValue(c);
                }
            }

            return sum;
        }
    }
}
