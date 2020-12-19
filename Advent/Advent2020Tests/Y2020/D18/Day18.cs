using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Advent2020;
using AdventLibrary;

namespace Advent2020Tests.Days.D18
{
    [TestClass]
    public class Day18
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D18/Data.txt");
        }

        public Expression[] GetData()
        {
            return GetLines().Select(l =>
            {
                l = l.Replace("(", "( ").Replace(")", " )");
                string[] parts = l.Split(' ').Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
                return new Expression(parts);
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            long actual = Problem1(GetData());
            Console.WriteLine(actual);
            Assert.AreEqual(30753705453324, actual);
        }

        private long Problem1(Expression[] lines)
        {
            long sum = 0;
            foreach (var e in lines)
            {
                long a = e.EvaluateLeft();
                //Console.WriteLine($"{e} ==> {a}");
                sum += a;
            }

            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            long actual = Problem2(GetData());
            Console.WriteLine(actual);
            Assert.AreEqual(244817530095503, actual);
        }

        private long Problem2(Expression[] lines)
        {
            long sum = 0;
            foreach (var e in lines)
            {
                long a = e.EvaluatePlusHigher();
                //Console.WriteLine($"{e} ==> {a}");
                sum += a;
            }

            return sum;
        }
    }

    public class Expression
    {
        public List<Token> AllTokens;
        public Range Range;
        public Expression(string[] tokens)
        {
            AllTokens = new List<Token>();
            int index = 0;
            foreach (string token in tokens)
            {
                AllTokens.Add(new Token(token, index++, this));
            }
            Range = new Range(0, tokens.Length - 1);
        }

        public Expression(List<Token> tokens, Range range)
        {
            AllTokens = tokens;
            Range = range;
        }

        public Token this[int i]
        {
            get
            {
                if (i < 0) throw new ArgumentOutOfRangeException("i < 0");
                if (i > Range.Max - Range.Min + 1) throw new ArgumentOutOfRangeException("i > Range");
                return AllTokens[i + Range.Min];
            }
        }

        public long EvaluateLeft()
        {
            return AllTokens.Last().EvaluateLeft(out int _);
        }

        public long EvaluatePlusHigher(int depth = 0)
        {
            var ret = EvaluatePlusHigherHelper(depth);
            //string s = "";
            //for (int i = 0; i < depth; i++) s += " ";
            //Console.WriteLine($"{s} {ret} : {this}");
            return ret;
        }

        public long EvaluatePlusHigherHelper(int depth)
        {
            // any 1 token expression must just be a value
            if (Range.Min == Range.Max) return long.Parse(this[Range.Min].Value);

            int startOutterParen = -1;
            int numParens = 0;
            for (int i = Range.Min; i <= Range.Max; i++)
            {
                Token t = AllTokens[i];
                if (t.Value == "(")
                {
                    if (startOutterParen == -1) startOutterParen = i;
                    numParens++;
                }
                else if (t.Value == ")")
                {
                    numParens--;
                    if (numParens == 0)
                    {
                        // found an outter paren.
                        Expression inParen = new Expression(AllTokens, new Range(startOutterParen + 1, i - 1));
                        long inParenValue = inParen.EvaluatePlusHigher(depth + 1);
                        string[] nextExpr = AllTokens.Where((t1, ti) => ti < startOutterParen && ti >= Range.Min)
                            .Concat(new[] {new Token(inParenValue.ToString(), 0, this)})
                            .Concat(AllTokens.Where((t1, ti) => ti > i && ti <= Range.Max))
                            .Select(t1 => t1.Value).ToArray();
                        return new Expression(nextExpr).EvaluatePlusHigher(depth + 1);
                    }
                }
                else if (t.Value == "*" && numParens == 0)
                {
                    // found an outter *
                    string[] left = AllTokens.Where((t1, ti) => ti < i && ti >= Range.Min).Select(t1 => t1.Value).ToArray();
                    string[] right = AllTokens.Where((t1, ti) => ti > i && ti <= Range.Max).Select(t1 => t1.Value).ToArray();
                    return new Expression(left).EvaluatePlusHigher(depth + 1) * new Expression(right).EvaluatePlusHigher(depth + 1);
                }
            }
            // no parens and no *'s, so just evaluate left.
            string[] theseTokens = AllTokens.Where((t1, ti) => ti <= Range.Max && ti >= Range.Min).Select(t1 => t1.Value).ToArray();
            return new Expression(theseTokens).EvaluateLeft();
        }

        public override string ToString() => $"{string.Join(" ", AllTokens.Skip(Range.Min).Take(Range.Max - Range.Min + 1))}";
    }

    public class Token
    {
        public string Value;
        public int Index;
        private Expression Expression;

        public Token(string value, int index, Expression expression)
        {
            Value = value;
            Index = index;
            Expression = expression;
        }

        public long EvaluateLeft(out int endOfThisPart)
        {
            if (Value == "(") throw new InvalidOperationException("unexpected '('");
            if (Value == "*") throw new InvalidOperationException("unexpected '*'");
            if (Value == "+") throw new InvalidOperationException("unexpected '*'");
            int endOfRight;
            long right;
            if (Value == ")")
            {
                right = Expression[Index - 1].EvaluateLeft(out endOfRight);
            }
            else
            {
                right = long.Parse(Value);
                endOfRight = Index;
            }

            if (endOfRight == 0)
            {
                endOfThisPart = 0;
                return right;
            }
            Token op = Expression[endOfRight - 1];

            if (op.Value == "(")
            {
                endOfThisPart = endOfRight - 1;
                return right;
            }

            long left = Expression[endOfRight - 2].EvaluateLeft(out endOfThisPart);
            if (op.Value == "+") return left + right;
            if (op.Value == "*") return left * right;
            throw new InvalidOperationException($"unexpected '{op}'");
        }

        public override string ToString() => $"{Value}";
    }
}
