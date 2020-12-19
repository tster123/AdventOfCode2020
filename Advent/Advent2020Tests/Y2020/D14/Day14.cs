using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AdventLibrary;

namespace Advent2020Tests.Days.D14
{
    [TestClass]
    public class Day14
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D14/Data.txt");
        }

        public object[] GetData()
        {
            return GetLines().Select(l =>
            {
                if (l.StartsWith("mask"))
                {
                    return new Mask(l.Split(' ').Last().Trim());
                }
                else
                {
                    string[] parts = l.Split(" = ");
                    return (object)new MemSet(int.Parse(parts[0].Substring(4).Replace("]", "")), uint.Parse(parts[1]));
                }
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(object[] lines)
        {
            string currentMask = null;
            Dictionary<int, long> memory = new Dictionary<int, long>();
            foreach (var t in lines)
            {
                Mask mask = t as Mask;
                if (mask != null)
                {
                    currentMask = mask.mask;
                }
                else
                {
                    MemSet memset = t as MemSet;
                    char[] value = ToString(memset.value);
                    for (int bit = 0; bit < currentMask.Length; bit++)
                    {
                        char maskBit = currentMask[bit];
                        if (maskBit == 'X') continue;
                        value[bit] = maskBit;
                    }

                    memory[memset.register] = ToValue(value);
                }
            }

            return memory.Values.Sum();
        }

        public long ToValue(char[] str)
        {
            return Convert.ToInt64(new string(str), 2);
        }

        public char[] ToString(long value)
        {
            var a = Convert.ToString(value, 2).ToCharArray();
            char[] ret = new char[36];
            for (int i = 0; i < 36; i++)
            {
                ret[i] = '0';
            }
            int target = 35;
            for (int i = a.Length - 1; i >= 0; i--)
            {
                ret[target] = a[i];
                target--;
            }

            return ret;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(object[] lines)
        {
            char[] currentMask = null;
            Dictionary<long, long> memory = new Dictionary<long, long>();
            foreach (var l in lines)
            {
                Mask mask = l as Mask;
                
                if (mask != null)
                {
                    currentMask = mask.mask.ToCharArray();
                }
                else
                {
                    MemSet memset = (MemSet)l;
                    var addresses = GetApplicableRegisters(memset.register, currentMask).ToList();
                    foreach (long address in addresses)
                    {
                        memory[address] = memset.value;
                    }
                }
            }

            return memory.Values.Sum();
        }

        private IEnumerable<long> GetApplicableRegisters(int register, char[] currentMask)
        {

            char[] registers = ToString(register);
            List<int> floating = new List<int>();
            for (int i = 0; i < 36; i++)
            {
                if (currentMask[i] == '1') registers[i] = '1';
                if (currentMask[i] == 'X') floating.Add(i);
            }

            return ExpandFloating(registers, floating, 0).Select(r => ToValue(r));
        }

        private IEnumerable<char[]> ExpandFloating(char[] registers, List<int> floating, int floatingIndex)
        {
            if (floatingIndex >= floating.Count)
            {
                yield return registers;
                yield break;
            }
            foreach (var register in ExpandFloating(registers, floating, floatingIndex + 1))
            {
                var a = register.ToArray();
                a[floating[floatingIndex]] = '0';
                yield return a;
                var b = register.ToArray();
                b[floating[floatingIndex]] = '1';
                yield return b;
            }
        }
    }

    public class Mask
    {
        public string mask;

        public Mask(string mask)
        {
            this.mask = mask;
        }
    }

    public class MemSet
    {
        public int register;
        public long value;

        public MemSet(int register, long value)
        {
            this.register = register;
            this.value = value;
        }
    }
}
