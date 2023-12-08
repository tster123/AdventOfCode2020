using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2023.D1
{
    [TestClass]
    public class Day1 : AdventTest
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
            int sum = 0;
            foreach (string line in lines)
            {
                var chars = line.Where(c => c >= '0' && c <= '9').ToArray();
                sum += int.Parse(chars.First() + "" + chars.Last());
            }
            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string orig = lines[i];
                string b = "";
                for (int j = 0; j < orig.Length; j++)
                {
                    if (orig[j] >= '0' && orig[j] <= '9')
                    {
                        b += orig[j];
                    }
                    else
                    {
                        string check = orig.Substring(j);
                        if (check.StartsWith("one")) b += "1";
                        if (check.StartsWith("two")) b += "2";
                        if (check.StartsWith("three")) b += "3";
                        if (check.StartsWith("four")) b += "4";
                        if (check.StartsWith("five")) b += "5";
                        if (check.StartsWith("six")) b += "6";
                        if (check.StartsWith("seven")) b += "7";
                        if (check.StartsWith("eight")) b += "8";
                        if (check.StartsWith("nine")) b += "9";
                    }
                }

                lines[i] = b;
            }

            return Problem1(lines);
        }
    }
}
