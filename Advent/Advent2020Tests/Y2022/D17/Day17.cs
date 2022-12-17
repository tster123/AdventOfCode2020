using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Advent2020;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2022.D17
{
    [TestClass]
    public class Day17 : AdventTest
    {

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetLines().First()));
        }

        private object Problem1(string input)
        {
            var map = Sim(input, 2022);
            return map.Count;
        }

        private List<char[]> Sim(string input, long numRocks, int rockTypeOffset = 0, int jetIndex = 0, List<char[]> map = null)
        {
            if (map == null) map = new List<char[]>();
            for (int i = 0; i < numRocks; i++)
            {
                List<Point2D<char>> fallingRock = AddShape((i + rockTypeOffset) % 5, map);
                //Print(map);
                if (debug) Print(map);
                DropShape(map, input, ref jetIndex, fallingRock);
            }

            for (int y = map.Count - 1; y > 0; y--)
            {
                if (new string(map[y]) == ".......")
                {
                    map.RemoveAt(y);
                }
                else
                {
                    break;
                }
            }
            //Print(map);
            return map;
        }

        private void Print(List<char[]> map)
        {
            for (int y = map.Count - 1; y >= 0; y--)
            {
                Console.WriteLine(new string(map[y]));
            }

            Console.WriteLine();
        }

        private void DropShape(List<char[]> map, string jets, ref int jetIndex, List<Point2D<char>> fallingRock)
        {
            
            while (true)
            {
                fallingRock = DoJet(map, jets, ref jetIndex, fallingRock);
                //Print(map);
                if (DoFall(map, ref fallingRock)) return;
                //Print(map);
            }
            

            throw new NotImplementedException();
        }

        private static bool DoFall(List<char[]> map, ref List<Point2D<char>> fallingRock)
        {
            bool canFall = true;
            foreach (var p in fallingRock)
            {
                int x = p[1];
                int y = p[0];

                if (y < 1 || map[y - 1][x] == '#')
                {
                    canFall = false;
                    break;
                }
            }

            if (!canFall)
            {
                foreach (var p in fallingRock)
                {
                    int x = p[1];
                    int y = p[0];
                    map[y][x] = '#';
                }

                return true;
            }

            var newFallingRock2 = new List<Point2D<char>>();
            foreach (var p in fallingRock)
            {
                int x = p[1];
                int y = p[0];
                map[y][x] = '.';
            }

            foreach (var p in fallingRock)
            {
                int x = p[1];
                int y = p[0];
                map[y - 1][x] = '@';
                newFallingRock2.Add(new Point2D<char>(y - 1, x));
            }

            fallingRock = newFallingRock2;
            return false;
        }

        private static List<Point2D<char>> DoJet(List<char[]> map, string jets, ref int jetIndex, List<Point2D<char>> fallingRock)
        {
            bool isLeft = jets[jetIndex] == '<';
            jetIndex++;
            if (jetIndex >= jets.Length) jetIndex = 0;

            int xDiff = isLeft ? -1 : 1;
            bool canJet = true;
            foreach (var p in fallingRock)
            {
                int x = p[1];
                int y = p[0];

                int newX = x + xDiff;
                if (newX < 0 || newX >= 7 || map[y][newX] == '#')
                {
                    canJet = false;
                    break;
                }
            }

            if (canJet)
            {
                var newFallingRock = new List<Point2D<char>>();
                foreach (var p in fallingRock)
                {
                    int x = p[1];
                    int y = p[0];
                    map[y][x] = '.';
                }

                foreach (var p in fallingRock)
                {
                    int x = p[1];
                    int y = p[0];
                    map[y][x + xDiff] = '@';
                    newFallingRock.Add(new Point2D<char>(y, x + xDiff));
                }

                fallingRock = newFallingRock;
            }

            return fallingRock;
        }

        public List<Point2D<char>> AddShape(int index, List<char[]> map)
        {
            for (int y = map.Count - 1; y > 0; y--)
            {
                if (new string(map[y]) == ".......")
                {
                    map.RemoveAt(y);
                }
                else
                {
                    {
                        break;
                    }
                }
            }

            map.Add(new[] { '.', '.', '.', '.', '.', '.', '.' });
            map.Add(new[] { '.', '.', '.', '.', '.', '.', '.' });
            map.Add(new[] { '.', '.', '.', '.', '.', '.', '.' });


            // ####
            if (index % 5== 0)
            {
                map.Add(new[] { '.', '.', '@', '@', '@', '@', '.' });
            }
            // .#.
            // ###
            // .#.
            if (index % 5 == 1)
            {
                map.Add(new[] { '.', '.', '.', '@', '.', '.', '.' });
                map.Add(new[] { '.', '.', '@', '@', '@', '.', '.' });
                map.Add(new[] { '.', '.', '.', '@', '.', '.', '.' });
            }
            // ..#
            // ..#
            // ###
            if (index % 5 == 2)
            {
                map.Add(new[] { '.', '.', '@', '@', '@', '.', '.' });
                map.Add(new[] { '.', '.', '.', '.', '@', '.', '.' });
                map.Add(new[] { '.', '.', '.', '.', '@', '.', '.' });
            }

            // #
            // #
            // #
            // #
            if (index % 5 == 3)
            {
                map.Add(new[] { '.', '.', '@', '.', '.', '.', '.' });
                map.Add(new[] { '.', '.', '@', '.', '.', '.', '.' });
                map.Add(new[] { '.', '.', '@', '.', '.', '.', '.' });
                map.Add(new[] { '.', '.', '@', '.', '.', '.', '.' });
            }
            // ##
            // ##
            if (index % 5 == 4)
            {
                map.Add(new[] { '.', '.', '@', '@', '.', '.', '.' });
                map.Add(new[] { '.', '.', '@', '@', '.', '.', '.' });
            }

            var ret = new List<Point2D<char>>();
            for (int y = map.Count - 1; y >= 0; y--)
            {
                bool foundOne = false;
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '@')
                    {
                        ret.Add(new Point2D<char>(y, x));
                        foundOne = true;
                    }
                }

                if (!foundOne)
                {
                    break;
                }
            }
            return ret;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetLines().First()));
        }

        class TopInfo
        {
            public long BlockCount;
            public long Height;

            public TopInfo(long blockCount, long height)
            {
                BlockCount = blockCount;
                Height     = height;
            }
        }
        private object Problem2(string input)
        {
            List<char[]> map = new List<char[]>();
            var topToInfo = new Dictionary<string, TopInfo>();
            int jetIndex = 0;
            for (int i = 0; i < 1000000; i++)
            {
                List<Point2D<char>> fallingRock = AddShape(i, map);
                DropShape(map, input, ref jetIndex, fallingRock);

                for (int y = map.Count - 1; y > 0; y--)
                {
                    if (new string(map[y]) == ".......")
                    {
                        map.RemoveAt(y);
                    }
                    else
                    {
                        break;
                    }
                }

                // build top
                string top = BuildTop(map);
                if (top == null) continue;

                top = jetIndex + "\n" + (i%5) + "\n" + top;
                //Console.WriteLine(top);
                if (topToInfo.ContainsKey(top))
                {
                    var ti = topToInfo[top];
                    //Console.WriteLine($"FOUND REPEAT: at i={i}, oldI={ti.BlockCount}, height={map.Count}, oldHeight={ti.Height} on top\n{top}");
                    long numDropsDistance = (i + 1) - topToInfo[top].BlockCount;
                    long remaining = 1000000000000 - (i + 1);
                    long numRepeat = remaining / numDropsDistance;
                    long height = map.Count + numRepeat * (map.Count - ti.Height);
                    remaining = remaining % numDropsDistance;
                    
                    var topAsMap = top.Split("\n").Skip(2).Select(s => s.Trim()).Where(s => s.Length > 0).Select(s => s.ToCharArray()).ToList();
                    topAsMap.Reverse();
                    //Print(topAsMap);
                    long topHeight = topAsMap.Count;
                    //debug = true;
                    var finalMap = Sim(input, remaining, 1 + (i % 5), jetIndex, topAsMap);
                    //Print(finalMap);
                    return height + (finalMap.Count - topHeight);
                }

                topToInfo[top] = new TopInfo(i + 1, map.Count);
            }

            return null;
        }

        private bool debug = false;
        private string BuildTop(List<char[]> map)
        {
            char[] flow = map[map.Count - 1].ToArray();
            StringBuilder sb = new StringBuilder(new string(flow));
            sb.AppendLine();
            int y = map.Count - 1;
            while (flow.Contains('.'))
            {
                y--;
                if (y <= 0) return null;
                char[] nextLine = map[y];
                sb.AppendLine(new string(nextLine));
                for (int i = 0; i < 7; i++)
                {
                    if (flow[i] == '.' && nextLine[i] == '#') flow[i] = '#';
                }
                for (int i = 0; i < 6; i++)
                {
                    if (flow[i] == '.' && nextLine[i + 1] == '.') flow[i + 1] = '.';
                }
                for (int i = 6; i > 0; i--)
                {
                    if (flow[i] == '.' && nextLine[i - 1] == '.') flow[i - 1] = '.';
                }
            }

            return sb.ToString();
        }
    }
}
