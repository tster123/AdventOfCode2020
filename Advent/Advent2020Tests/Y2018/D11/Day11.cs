using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Threading.Tasks;
using AdventLibrary;

namespace Advent2020Tests.Y2018.D11
{
    [TestClass]
    public class Day11
    {
        public const int SerialNumber = 3463;

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1A());
        }

        private object Problem1A()
        {
            int max = -1000;
            string maxStr = null;
            for (int x = 1; x <= 297; x++)
            for (int y = 1; y <= 297; y++)
            {
                int sum = 0;
                for (int ix = 0; ix < 3; ix++)
                for (int iy = 0; iy < 3; iy++)
                {
                    sum += new FuelCell(x + ix, y + iy).PowerLevel;
                }

                if (sum > max)
                {
                    max = sum;
                    maxStr = x + "," + y;
                }
            }

            return maxStr;
        }

        [TestMethod]
        public void Problem2()
        {
            string ret = Problem2A();
            Assert.AreEqual("233,282,11", ret);
            Console.WriteLine(ret);
        }

        private string Problem2A()
        {
            int max = -1000;
            string maxStr = null;
            int[,] matrix = new int[301, 301];
            for (int x = 1; x <= 300; x++)
            for (int y = 1; y <= 300; y++)
            {
                matrix[x, y] = new FuelCell(x, y).PowerLevel;
            }

            var mem = new Dictionary<string, int>();

            //Parallel.For(1, 301, size =>
            for (int size = 1; size <= 300; size++)
            {
                if (size == 1)
                {
                    for (int x = 1; x <= 301 - size; x++)
                    for (int y = 1; y <= 301 - size; y++)
                    {
                        int sum = 0;
                        for (int ix = 0; ix < size; ix++)
                        for (int iy = 0; iy < size; iy++)
                        {
                            sum += matrix[x + ix, y + iy];
                        }

                        string label = x + "," + y + "," + size;
                        mem[label] = sum;
                        if (sum > max)
                        {
                            lock (this)
                            {
                                if (sum > max)
                                {
                                    max = sum;
                                    maxStr = label;
                                }
                            }
                        }
                    }

                }
                else
                {
                    for (int x = 1; x <= 301 - size; x++)
                    for (int y = 1; y <= 301 - size; y++)
                    {
                        int sum = mem[x + "," + y + "," + (size - 1)];
                        for (int ix = 0; ix < size - 1; ix++)
                        {
                            sum += matrix[x + ix, y + size - 1];
                        }

                        for (int iy = 0; iy < size - 1; iy++)
                        {
                            sum += matrix[x + size - 1, y + iy];
                        }

                        sum += matrix[x + size - 1, y + size - 1];
                        string label = x + "," + y + "," + size;
                        mem[label] = sum;
                        if (sum > max)
                        {
                            lock (this)
                            {
                                if (sum > max)
                                {
                                    max = sum;
                                    maxStr = label;
                                }
                            }
                        }
                    }
                }
            //}); // uncomment for parallel
            } // uncomment for non-parallel

            return maxStr;
        }
    }

    public class FuelCell
    {
        private int X, Y;
        public readonly int PowerLevel;
        public FuelCell(int x, int y)
        {
            X = x;
            Y = y;
            int level = RackId * Y;
            level += Day11.SerialNumber;
            level *= RackId;
            level = (level / 100) % 10;
            level -= 5;
            PowerLevel = level;
        }

        public int RackId => X + 10;
    }
}
