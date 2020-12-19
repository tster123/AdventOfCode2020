using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2020.D19
{
    [TestClass]
    public class Day19
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D19/Data.txt");
        }

        public Data GetData(bool part2)
        {
            bool onRules = true;
            List<Rule> roughtRules = new List<Rule>();
            List<string> input = new List<string>();
            foreach (string line in GetLines())
            {
                if (line.Trim() == "")
                {
                    onRules = false;
                    continue;
                }
                if (onRules)
                {
                    string modLine = line;
                    if (part2)
                    {
                        if (modLine.StartsWith("8:")) modLine = "8: 42 | 42 8";
                        if (modLine.StartsWith("11:")) modLine = "11: 42 31 | 42 11 31";
                    }

                    var parts = modLine.Split(": ");
                    roughtRules.Add(new Rule(int.Parse(parts[0]), parts[1]));
                }
                else
                {
                    input.Add(line);
                }
            }

            Rule[] rules = new Rule[roughtRules.Select(r => r.Num).Max() + 1];
            foreach (Rule r in roughtRules)
            {
                rules[r.Num] = r;
            }

            return new Data(rules, input);
        }

        [TestMethod]
        public void Problem1()
        {
            object expected = 230;
            object actual = Problem2(GetData(false));
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Problem2()
        {
            object expected = 341;
            object actual = Problem2(GetData(true));
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        private object Problem2(Data data)
        {
            var compiled = data.Rules[0].GetCompiledRule(data.Rules);
            compiled.Compile();
            int count = 0;
            foreach (string line in data.Lines)
            {
                if (compiled.Matches(line, 0).Any(i => i == line.Length - 1))
                {
                    count++;
                }
            }
            return count;
        }
    }

    public class Data
    {
        public Rule[] Rules;
        public List<string> Lines;

        public Data(Rule[] rules, List<string> lines)
        {
            Rules = rules;
            Lines = lines;
        }
    }

    public interface ICompiledRule
    {
        /// <summary>
        /// Returns the list of end indexes that match this compiled rule
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        IEnumerable<int> Matches(string line, int pos);

        void Compile();

        string Text { get; }
    }

    public class BasicRuleMatch : ICompiledRule
    {
        private char Char;
        public string Text => ToString();

        public BasicRuleMatch(char c)
        {
            Char = c;
        }

        public IEnumerable<int> Matches(string line, int pos)
        {
            if (line[pos] == Char) return new[] {pos};
            return new int[0];
        }

        public void Compile()
        {
        }

        public override string ToString() => $"\"{Char}\"";
    }

    public class OrMatch : ICompiledRule
    {
        private Func<ICompiledRule> leftGet, rightGet;
        private ICompiledRule left, right;
        public string Text { get; }

        public OrMatch(Func<ICompiledRule> left, Func<ICompiledRule> right, string text)
        {
            this.leftGet = left;
            this.rightGet = right;
            Text = text;
        }

        public void Compile()
        {
            if (left == null)
            {
                left = leftGet();
                left.Compile();
            }

            if (right == null)
            {
                right = rightGet();
                right.Compile();
            }
        }

        public IEnumerable<int> Matches(string line, int pos)
        {
            bool[] returned = new bool[line.Length];
            foreach (int ret in left.Matches(line, pos))
            {
                if (!returned[ret]) yield return ret;
                returned[ret] = true;
            }
            foreach (int ret in right.Matches(line, pos))
            {
                if (!returned[ret]) yield return ret;
                returned[ret] = true;
            }
        }

        public override string ToString() => $"{Text}";
    }

    public class RuleMatch : ICompiledRule
    {
        private Func<ICompiledRule> leftGet, rightGet;
        private ICompiledRule left, right;

        public string Text { get; }
        public RuleMatch(Func<ICompiledRule> left, Func<ICompiledRule> right, string text)
        {
            this.leftGet = left;
            this.rightGet = right;
            this.Text = text;
        }

        public void Compile()
        {
            if (left == null)
            {
                left = leftGet();
                left.Compile();
            }

            if (right == null)
            {
                right = rightGet();
                right?.Compile();
            }
        }

        public IEnumerable<int> Matches(string line, int pos)
        {
            var lefts = left.Matches(line, pos);
            if (right == null)
            {
                foreach (int i in lefts) yield return i;
                yield break;
            }

            bool[] returned = new bool[line.Length];
            foreach (int leftPos in lefts)
            {
                // left took us too far
                if (leftPos >= line.Length - 1) continue;

                foreach (int rightPos in right.Matches(line, leftPos + 1))
                {
                    if (!returned[rightPos]) yield return rightPos;
                    returned[rightPos] = true;
                }
            }
        }

        public override string ToString() => $"{Text}";
    }

    public class Rule
    {
        public int Num;
        public string Text;

        private HashSet<string> matches;

        public Rule(int num, string text)
        {
            Num = num;
            Text = text;
        }

        private ICompiledRule compiled;

        public ICompiledRule GetCompiledRule(Rule[] rules)
        {
            if (compiled == null) compiled = CompileThing(Text, rules);
            return compiled;
        }

        private static ICompiledRule CompileThing(string thing, Rule[] rules)
        {
            if (thing.StartsWith("\"")) return new BasicRuleMatch(thing[1]);
            if (thing.Contains("|"))
            {
                string[] parts = thing.Split(" | ");
                string left = parts[0];
                string right = parts[1];
                return new OrMatch(() => CompileThing(left, rules), () => CompileThing(right, rules), thing);
            }
            else
            {
                string[] parts = thing.Split(" ");
                Func<ICompiledRule> right = () => null;
                if (parts.Length > 1)
                {
                    string rightText = string.Join(" ", parts.Skip(1));
                    right = () => CompileThing(rightText, rules);
                }
                return new RuleMatch(() => rules[int.Parse(parts[0])].GetCompiledRule(rules), right, thing);
            }
        }

        public HashSet<string> GetMatches(Rule[] rules)
        {
            if (matches != null) return matches;
            matches = new HashSet<string>();
            if (Text.Contains("|"))
            {
                string[] parts = Text.Split(" | ");
                string left = parts[0];
                string right = parts[1];
                AddMatches(left, rules);
                AddMatches(right, rules);
            }
            else
            {
                AddMatches(Text, rules);
            }

            return matches;
        }

        private void AddMatches(string text, Rule[] rules)
        {
            if (text.StartsWith("\""))
            {
                matches.Add(text[1].ToString());
                return;
            }

            int[] parts = text.Split(" ").Select(s => int.Parse(s.Trim())).ToArray();

            if (parts.Length == 1)
            {
                foreach (var m in rules[parts[0]].GetMatches(rules))
                    matches.Add(m);
                return;
            }

            string[] left = rules[parts[0]].GetMatches(rules).ToArray();
            string[] right = rules[parts[1]].GetMatches(rules).ToArray();
            foreach (string l in left)
            foreach (string r in right)
            {
                matches.Add(l + r);
            }
        }
    }
}
