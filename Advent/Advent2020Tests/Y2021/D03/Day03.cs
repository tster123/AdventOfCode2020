using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2021.D03
{
    [TestClass]
    public class Day03 : AdventTest
    {

        public string[] GetData()
        {
            return GetLines().Select(l =>
            {
                return l;
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(string[] lines)
        {
            var gamma = BuildGamma(lines);
            var epsilon = BuildEpsilon(gamma);

            int ret =  gamma.ToInt(2) * epsilon.ToInt(2);
            Assert.AreEqual(3813416, ret);
            return ret;
        }

        private static string BuildEpsilon(string gamma)
        {
            int[] epsilon = new int[gamma.Length];
            for (int i = 0; i < gamma.Length; i++)
            {
                epsilon[i] = gamma[i] == '0' ? 1 : 0;
            }

            return string.Join("", epsilon);
        }

        private static string BuildGamma(string[] lines)
        {
            int[] gamma = new int[lines[0].Length];
            foreach (var line in lines)
            {
                for (int i = 0; i < gamma.Length; i++)
                {
                    gamma[i] += line[i] == '0' ? -1 : 1;
                }
            }

            for (int i = 0; i < gamma.Length; i++)
            {
                gamma[i] = gamma[i] < 0 ? 0 : 1;
            }

            return string.Join("", gamma);
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(string[] lines)
        {
            string[] oxygenCandidates = lines;
            string[] scrubberCandidates = lines;
            for (int i = 0; i < lines[0].Length; i++)
            {
                if (oxygenCandidates.Length > 1)
                {
                    string g = BuildGamma(oxygenCandidates);
                    oxygenCandidates = oxygenCandidates.Where(s => s[i] == g[i]).ToArray();
                }

                if (scrubberCandidates.Length > 1)
                {
                    string g = BuildGamma(scrubberCandidates);
                    string e = BuildEpsilon(g);
                    scrubberCandidates = scrubberCandidates.Where(s => s[i] == e[i]).ToArray();
                }
            }

            return oxygenCandidates[0].ToInt(2) * scrubberCandidates[0].ToInt(2);
        }
    }
}
