using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Advent2020Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2020.D22
{
    [TestClass]
    public class Day22 : AdventTest
    {

        public Game GetTest()
        {
            return new Game(new List<int>
                {
                    9,2,6,3,1
                },
                new List<int>
                {
                    5,8,4,7,10
                });
        }

        public Game GetData()
        {
            var p1 = new List<int>();
            var p2 = new List<int>();
            var on = p1;
            foreach (string s in GetLines())
            {
                if (s.Equals("Player 2:"))
                {
                    on = p2;
                }

                if (int.TryParse(s, out int v)) on.Add(v);
            }
            return new Game(p1,p2);
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Game game)
        {
            while (game.Player1.Count > 0 && game.Player2.Count > 0)
            {
                int p1 = game.Player1[0];
                int p2 = game.Player2[0];
                game.Player1.RemoveAt(0);
                game.Player2.RemoveAt(0);
                List<int> give = p1 > p2 ? game.Player1 : game.Player2;
                int win = p1 > p2 ? p1 : p2;
                int loss = p1 > p2 ? p2 : p1;
                give.Add(win);
                give.Add(loss);
            }

            return Score(game.Player1.Count > 0 ? game.Player1 : game.Player2);
        }

        private static object Score(List<int> winner)
        {
            
            long score = 0;
            for (int i = 0; i < winner.Count; i++)
            {
                score += ((i + 1) * winner[winner.Count - i - 1]);
            }

            return score;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Game game)
        {
            return Score(PlayGame(game).Winner);
        }

        private int hitMemoized = 0;
        private readonly Dictionary<int, Dictionary<string, bool>> memoryGlobal = new Dictionary<int, Dictionary<string, bool>>();
        private GameResult PlayGame(Game game)
        {
            for (int i = 0; i <= game.GameSize; i++)
            {
                memoryGlobal[i] = new Dictionary<string, bool>();
            }

            bool p1Win = PlayGameInternal(game);
            return new GameResult(game, p1Win);
        }

        private bool PlayGameInternal(Game game)
        {
            HashSet<string> seen = new HashSet<string>();
            var memory = memoryGlobal[game.GameSize];
            string toMem = game.ToString();
            while (game.Player1.Count > 0 && game.Player2.Count > 0)
            {
                string str = game.ToString();
                if (seen.Contains(str))
                {
                    memory[toMem] = true;
                    return memory[toMem];
                }
                seen.Add(str);
                if (memory.TryGetValue(str, out var ret))
                {
                    hitMemoized++;
                    return ret;
                }
                int p1 = game.Player1[0];
                int p2 = game.Player2[0];
                game.Player1.RemoveAt(0);
                game.Player2.RemoveAt(0);

                bool p1Win;
                if (game.Player1.Count >= p1 && game.Player2.Count >= p2)
                {
                    p1Win = PlayGameInternal(new Game(game.Player1.Take(p1).ToList(), game.Player2.Take(p2).ToList()));
                }
                else
                {
                    p1Win = p1 > p2;
                }
                List<int> give = p1Win ? game.Player1 : game.Player2;
                int win = p1Win ? p1 : p2;
                int loss = p1Win ? p2 : p1;
                give.Add(win);
                give.Add(loss);
            }
            /*foreach (string s in seen)
            {
                memory[s] = result;
            }*/
            memory[toMem] = game.Player1.Count > game.Player2.Count;

            return game.Player1.Count > game.Player2.Count;
        }
    }

    public class GameResult
    {
        public Game Game;
        public bool Player1Win;

        public GameResult(Game game, bool player1Win)
        {
            Game = game;
            Player1Win = player1Win;
        }
        public List<int> Winner => Player1Win ? Game.Player1 : Game.Player2;
    }

    public class Game
    {
        public List<int> Player1, Player2;

        public Game(List<int> player1, List<int> player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public int GameSize => Player1.Count + Player2.Count;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Player1.Count + 1 + Player2.Count);
            foreach (int i in Player1)
            {
                sb.Append((char) i);
            }

            sb.Append('|');
            foreach (int i in Player2)
            {
                sb.Append((char)i);
            }

            return sb.ToString();
        }
    }
}
