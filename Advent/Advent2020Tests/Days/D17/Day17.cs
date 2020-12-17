using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using AdventLibrary;

namespace Advent2020Tests.Days.D17
{
    [TestClass]
    public class Day17
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D17/Data.txt");
        }

        public Map2D<char> GetData()
        {
            return MapFactories.Character(GetLines());
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private Dictionary<int, Dictionary<int, Dictionary<int, bool>>> space =
            new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

        private bool Read(int x, int y, int z)
        {
            if (space.ContainsKey(x) && space[x].ContainsKey(y) && space[x][y].ContainsKey(z))
            {
                return space[x][y][z];
            }
            return false;
        }


        private Dictionary<int, Dictionary<int, Dictionary<int, bool>>> nextSpace =
            new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

        private void Set(int x, int y, int z, bool val)
        {
            if (!nextSpace.ContainsKey(x))
            {
                nextSpace[x] = new Dictionary<int, Dictionary<int, bool>>();
            }

            if (!nextSpace[x].ContainsKey(y)) nextSpace[x][y] = new Dictionary<int, bool>();
            nextSpace[x][y][z] = val;
        }

        private void CopySpace()
        {
            space = nextSpace;
            nextSpace = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();
        }

        private object Problem1(Map2D<char> start)
        {
            foreach (var p in start.GetAllPoints())
            {
                Set(p.X,p.Y,0, p.Value == '#');
            }

            CopySpace();

            for (int i = 0; i < 6; i++)
            {
                Tick();
                CopySpace();
                //Console.WriteLine("After cycle " + (i + 1));
                //Print();
            }
            return space.Values.SelectMany(a => a.Values).SelectMany(a => a.Values).Count(a => a);
        }

        private void Tick()
        {
            int minX = space.Keys.Min();
            int maxX = space.Keys.Max();
            int minY = space.Values.SelectMany(a => a.Keys).Min();
            int maxY = space.Values.SelectMany(a => a.Keys).Max();
            int minZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Min();
            int maxZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Max();
            for (int z = minZ - 1; z <= maxZ + 1; z++)
            for (int y = minY - 1; y <= maxY + 1; y++)
            for (int x = minX - 1; x <= maxX + 1; x++)
            {
                Tick(x, y, z);
            }
        }

        private void Print()
        {
            int minX = space.Keys.Min();
            int maxX = space.Keys.Max();
            int minY = space.Values.SelectMany(a => a.Keys).Min();
            int maxY = space.Values.SelectMany(a => a.Keys).Max();
            int minZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Min();
            int maxZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Max();
            for (int z = minZ; z <= maxZ; z++)
            {
                Console.WriteLine("z=" + z);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Console.Write(Read(x, y, z) ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }

        private void Tick(int x, int y, int z)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            for (int dz = -1; dz <= 1; dz++)
            {
                if (dx == 0 && dy == 0 && dz == 0) continue;
                if (Read(x + dx,y + dy,z + dz)) count++;
            }

            if (Read(x, y, z))
            {
                Set(x, y, z, count == 2 || count == 3);
            }
            else
            {
                Set(x, y, z, count == 3);
            }
        }
    }

    [TestClass]
    public class Day17Part2
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Days/D17/Data.txt");
        }

        public Map2D<char> GetData()
        {
            return MapFactories.Character(GetLines());
        }

        private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> space =
            new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();

        private bool Read(int x, int y, int z, int w)
        {
            if (space.ContainsKey(x) && space[x].ContainsKey(y) && space[x][y].ContainsKey(z) && space[x][y][z].ContainsKey(w))
            {
                return space[x][y][z][w];
            }
            return false;
        }


        private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> nextSpace =
            new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();

        private void Set(int x, int y, int z, int w, bool val)
        {
            if (!nextSpace.ContainsKey(x)) nextSpace[x] = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();
            if (!nextSpace[x].ContainsKey(y)) nextSpace[x][y] = new Dictionary<int, Dictionary<int, bool>>();
            if (!nextSpace[x][y].ContainsKey(z)) nextSpace[x][y][z] = new Dictionary<int, bool>();
            nextSpace[x][y][z][w] = val;
        }

        private void CopySpace()
        {
            space = nextSpace;
            nextSpace = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();
        }

        private void Tick()
        {
            int minX = space.Keys.Min();
            int maxX = space.Keys.Max();
            int minY = space.Values.SelectMany(a => a.Keys).Min();
            int maxY = space.Values.SelectMany(a => a.Keys).Max();
            int minZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Min();
            int maxZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Max();
            int minW = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Values).SelectMany(a => a.Keys).Min();
            int maxW = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Values).SelectMany(a => a.Keys).Max();
            for (int z = minZ - 1; z <= maxZ + 1; z++)
            for (int y = minY - 1; y <= maxY + 1; y++)
            for (int x = minX - 1; x <= maxX + 1; x++)
            for (int w = minW - 1; w <= maxW + 1; w++)
            {
                Tick(x, y, z, w);
            }
        }
        /*
        private void Print()
        {
            int minX = space.Keys.Min();
            int maxX = space.Keys.Max();
            int minY = space.Values.SelectMany(a => a.Keys).Min();
            int maxY = space.Values.SelectMany(a => a.Keys).Max();
            int minZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Min();
            int maxZ = space.Values.SelectMany(a => a.Values).SelectMany(a => a.Keys).Max();
            for (int z = minZ; z <= maxZ; z++)
            {
                Console.WriteLine("z=" + z);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Console.Write(Read(x, y, z) ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }
        */
        private void Tick(int x, int y, int z, int w)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            for (int dz = -1; dz <= 1; dz++)
            for (int dw = -1; dw <= 1; dw++)
            {
                if (dx == 0 && dy == 0 && dz == 0 && dw == 0) continue;
                if (Read(x + dx, y + dy, z + dz, w + dw)) count++;
            }

            if (Read(x, y, z, w))
            {
                Set(x, y, z, w, count == 2 || count == 3);
            }
            else
            {
                Set(x, y, z, w, count == 3);
            }
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(Map2D<char> start)
        {
            foreach (var p in start.GetAllPoints())
            {
                Set(p.X, p.Y, 0, 0, p.Value == '#');
            }

            CopySpace();

            for (int i = 0; i < 6; i++)
            {
                Tick();
                CopySpace();
                //Console.WriteLine("After cycle " + (i + 1));
                //Print();
            }
            return space.Values.SelectMany(a => a.Values).SelectMany(a => a.Values).SelectMany(a => a.Values).Count(a => a);
        }
    }
}
