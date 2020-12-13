using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventLibrary;

namespace Advent2020Tests.Days.D12
{
    [TestClass]
    public class Day12
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D12/Data.txt");
        }

        public CommandC[] GetData()
        {
            return GetLines().Select(l =>
            {
                return new CommandC(l[0], int.Parse(l.Substring(1)));
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(CommandC[] lines)
        {
            char facing = 'E';
            int eDist = 0;
            int nDist = 0;
            var rightTurns = new Dictionary<char, char> {['N'] = 'E', ['E'] = 'S', ['S'] = 'W', ['W'] = 'N'};
            var leftTurns = new Dictionary<char, char> {['N'] = 'W', ['E'] = 'N', ['S'] = 'E', ['W'] = 'S'};
            foreach (var c in lines)
            {
                char a = c.Command;
                if (a == 'R')
                {
                    int times = c.Value / 90;
                    while (times > 0)
                    {
                        times--;
                        facing = rightTurns[facing];
                    }
                }

                if (a == 'L')
                {
                    int times = c.Value / 90;
                    while (times > 0)
                    {
                        times--;
                        facing = leftTurns[facing];
                    }
                }

                if (a == 'F')
                {
                    a = facing;
                }

                if (a == 'N')
                {
                    nDist += c.Value;
                }
                if (a == 'S')
                {
                    nDist -= c.Value;
                }
                if (a == 'E')
                {
                    eDist += c.Value;
                }
                if (a == 'W')
                {
                    eDist -= c.Value;
                }
            }
            return Math.Abs(nDist) + Math.Abs(eDist);
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(CommandC[] lines)
        {
            int eShip = 0;
            int nShip = 0;
            int eWaypt = 10;
            int nWaypt = 1;
            foreach (var c in lines)
            {
                char a = c.Command;
                if (a == 'L')
                {
                    c.Value = c.Value % 360;
                    c.Value = 360 - c.Value;
                    a = 'R';
                }

                if (a == 'R')
                {
                    int times = c.Value / 90;
                    while (times > 0)
                    {
                        int oldEast = eWaypt;
                        int oldNorth = nWaypt;

                        nWaypt = -1 * oldEast;
                        eWaypt = oldNorth;
                        times--;
                    }
                }

                if (a == 'F')
                {
                    nShip += c.Value * nWaypt;
                    eShip += c.Value * eWaypt;
                }

                if (a == 'N')
                {
                    nWaypt += c.Value;
                }
                if (a == 'S')
                {
                    nWaypt -= c.Value;
                }
                if (a == 'E')
                {
                    eWaypt += c.Value;
                }
                if (a == 'W')
                {
                    eWaypt -= c.Value;
                }
            }

            return Math.Abs(nShip) + Math.Abs(eShip);
        }
    }

    public class CommandC
    {
        public char Command;
        public int Value;

        public CommandC(char command, int value)
        {
            Command = command;
            Value = value;
        }
    }
}
