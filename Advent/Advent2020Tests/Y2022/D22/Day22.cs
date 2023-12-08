using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2022.D22
{
    [TestClass]
    public class Day22 : AdventTest
    {
        public class State
        {
            public Map2D<char> map;
            public List<char> turns;
            public List<int> walks;
            public Point2D<bool> location;
            public int facing; // 0 = right, 1 = down, 2 = left, 3 = up

        }

        public override string DataFile => "Test.txt";

        private void AddCommands(string commands, List<char> turns, List<int> walks)
        {
            int i = 0;
            while (i < commands.Length)
            {
                int nextL = commands.IndexOf('L', i);
                int nextR = commands.IndexOf('R', i);
                if (nextL == -1 && nextR == -1)
                {
                    walks.Add(int.Parse(commands.Substring(i)));
                    return;
                }
                if (nextL == -1) nextL = int.MaxValue;
                if (nextR == -1) nextR = int.MaxValue;
                int next = Math.Min(nextR, nextL);
                char nextTurn = nextL < nextR ? 'L' : 'R';
                walks.Add(int.Parse(commands.Substring(i, next - i)));
                turns.Add(nextTurn);
                i = next + 1;
            }
        }

        public State GetData()
        {
            string[] lines = GetLines();
            string commandLine = lines.Last();
            List<int> walks = new List<int>();
            var turns = new List<char>();
            AddCommands(commandLine, turns, walks);
            var map = new Map2D<char>(lines.Take(lines.Length - 2), '&');
            for (int x = 0; x < map.Width; x++)
            {
                if (map.Get(0, x) == '.')
                {
                    return new State
                    {
                        map = map,
                        facing = 0,
                        location = new Point2D<bool>(0, x),
                        turns = turns,
                        walks = walks
                    };
                }
            }

            throw new Exception("Cannot find first place");
        }
        private readonly int[][] facingVectors = new[]
        {
            new[] {0, 1},
            new[] {1, 0},
            new[] {0, -1},
            new[] {-1, 0}
        };

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(State state)
        {
            Console.WriteLine(state.location + " " + state.facing);
            for (int commandIndex = 0; commandIndex < state.walks.Count; commandIndex++)
            {
                int walk = state.walks[commandIndex];
                Walk(state, walk);
                if (state.location[0] < 0 || state.location[1] < 0)
                {
                    throw new Exception("Foo");
                }
                if (commandIndex < state.turns.Count)
                {
                    char turn = state.turns[commandIndex];
                    state.facing = ((state.facing + (turn == 'R' ? 1 : -1)) + 4) % 4;
                }

                Console.WriteLine(state.location + " " + state.facing + "  (" + walk + ")");
            }

            return (1000 * (1 + state.location[0])) + (4 * (1 + state.location[1])) + state.facing;
        }

        private void Walk(State state, int walk)
        {
            Point<bool> loc = state.location;
            var facingVector = facingVectors[state.facing];
            var reverse = new int[] { facingVector[0] * -1, facingVector[1] * -1 };
            for (int i = 0; i < walk; i++)
            {
                var nextLoc = loc.ApplyVector(facingVector);
                char val = state.map.Get(nextLoc);
                if (val == ' ' || val == '&')
                {
                    bool lastSeenIsWall = false;
                    // wrap around
                    Point<bool> lastMapTile = loc;
                    while (true)
                    {
                        nextLoc = nextLoc.ApplyVector(reverse);
                        if (state.map.Get(nextLoc) == '.' || state.map.Get(nextLoc) == '#')
                        {
                            lastMapTile = nextLoc;
                        }
                        else
                        {
                            break;
                        }
                    }

                    nextLoc = lastMapTile;
                }

                if (val == '#')
                {
                    break;
                }

                loc = nextLoc;
            }

            state.location = new Point2D<bool>(loc[0], loc[1]);
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(State state)
        {
            return null;
        }
    }
}
