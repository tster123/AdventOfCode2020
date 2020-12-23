using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventLibrary;

namespace Advent2020Tests.Y2020.D7
{
    [TestClass]
    public class Day7
    {
        public string[] GetData()
        {
            return File.ReadAllLines("./Y2020/D7/Data.txt");
        }

        //light gold bags contain 5 plaid turquoise bags, 2 dim indigo bags
        //dotted blue bags contain 5 muted violet bags
        public Rule Parse(string line)
        {
            string[] sides = line.Split(" bags contain ");
            string color = sides[0];
            if (sides[1] == "no other bags.") return new Rule(color, new Contains[0]);
            string[] a = sides[1].Split(',').Select(p => p.Trim()).ToArray();
            var contains = a.Select(r =>
            {
                r = r.Replace(".", "").Replace(" bags", "").Replace(" bag", "");
                string[] parts = r.Split(' ');
                int count = int.Parse(parts[0]);
                string c = r.Substring((count + " ").Length);
                return new Contains(c, count);
            });
            return new Rule(color, contains.ToArray());
        }

        [TestMethod]
        public void ParseTest()
        {
            Console.WriteLine(Parse("vibrant lavender bags contain 1 shiny coral bag, 4 dotted purple bags."));
            Console.WriteLine(Parse("dotted blue bags contain 5 muted violet bags."));
            Console.WriteLine(Parse("faded magenta bags contain 5 dull gold bags, 5 drab blue bags, 2 wavy maroon bags."));
            Console.WriteLine(Parse("faded blue bags contain no other bags."));
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(string[] lines)
        {
            Rule[] rules = lines.Select(l => Parse(l)).ToArray();
            HashSet<string> colors = new HashSet<string>();
            int count = 0;
            foreach (var rule in rules)
            {
                if (CanContain(rule.Color, "shiny gold", rules)) count++;
            }

            return count;
        }

        private bool CanContain(string outter, string inner, Rule[] rules)
        {
            Rule rule = rules.Single(r => r.Color == outter);
            foreach (var c in rule.Contains)
            {
                if (c.Color == inner) return true;
                if (CanContain(c.Color, inner, rules)) return true;
            }

            return false;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        public int HowManyInside(string color, Rule[] rules)
        {
            Rule rule = rules.Single(r => r.Color == color);
            int count = 0;
            foreach (var contains in rule.Contains)
            {
                count += contains.Count + contains.Count * HowManyInside(contains.Color, rules);
            }
            return count;
        }

        private object Problem2(string[] lines)
        {
            Rule[] rules = lines.Select(l => Parse(l)).ToArray();
            return HowManyInside("shiny gold", rules);
        }
    }

    public class Rule
    {
        public readonly string Color;
        public readonly Contains[] Contains;

        public Rule(string color, Contains[] contains)
        {
            Color = color;
            Contains = contains;
        }

        public override string ToString()
        {
            return
                $"{nameof(Color)}: {Color}, {nameof(Contains)}: {string.Join(", ", Contains.Select(c => "[" + c + "]"))}";
        }
    }

    public class Contains
    {
        public readonly string Color;
        public readonly int Count;

        public Contains(string color, int count)
        {
            Color = color;
            Count = count;
        }

        public override string ToString()
        {
            return $"{nameof(Color)}: {Color}, {nameof(Count)}: {Count}";
        }
    }
}
