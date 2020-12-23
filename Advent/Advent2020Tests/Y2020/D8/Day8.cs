using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using AdventLibrary;

namespace Advent2020Tests.Y2020.D8
{
    [TestClass]
    public class Day8
    {
        public string[] GetData()
        {
            return File.ReadAllLines("./Y2020/D8/Data.txt");
        }

        public Instruction[] Data()
        {
            return GetData().Select(l =>
            {
                var parts = l.Split(' ');
                return new Instruction((Type)Enum.Parse(typeof(Type), parts[0]), int.Parse(parts[1].Replace("+", "")));
            }).ToArray();
        }

        int RunToInfiniteLoop(Instruction[] program)
        {
            int value = 0;
            HashSet<int> linesRun = new HashSet<int>();
            int currentLine = 0;
            while (!linesRun.Contains(currentLine))
            {
                linesRun.Add(currentLine);
                Instruction i = program[currentLine];
                if (i.Type == Type.nop)
                {
                    currentLine++;
                }
                else if (i.Type == Type.acc)
                {
                    value += i.Value;
                    currentLine++;
                }
                else
                {
                    currentLine += i.Value;
                }
            }

            return value;
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(string[] lines)
        {
            return RunToInfiniteLoop(Data());
        }

        bool RunToCompletion(Instruction[] program, out int final)
        {
            int value = 0;
            HashSet<int> linesRun = new HashSet<int>();
            int currentLine = 0;
            while (true)
            {
                if (currentLine == program.Length)
                {
                    final = value;
                    return true;
                }

                if (currentLine > program.Length || linesRun.Contains(currentLine))
                {
                    final = -1;
                    return false;
                }
                linesRun.Add(currentLine);
                Instruction i = program[currentLine];
                if (i.Type == Type.nop)
                {
                    currentLine++;
                }
                else if (i.Type == Type.acc)
                {
                    value += i.Value;
                    currentLine++;
                }
                else
                {
                    currentLine += i.Value;
                }
            }
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(string[] lines)
        {
            Instruction[] program = Data();
            for (int i = 0; i < program.Length; i++)
            {
                Instruction ins = program[i];
                if (ins.Type == Type.acc) continue;
                Type orginal = ins.Type;
                ins.Type = orginal == Type.nop ? Type.jmp : Type.nop;
                int final;
                if (RunToCompletion(program, out final)) return final;
                ins.Type = orginal;
            }
            return "sadness";
        }
    }

    public enum Type
    {
        acc, jmp, nop
    }
    public class Instruction
    {
        public Type Type;
        public int Value;

        public Instruction(Type type, int value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Value)}: {Value}";
        }
    }
}
