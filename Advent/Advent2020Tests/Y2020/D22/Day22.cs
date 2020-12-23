using System.Collections.Generic;
using System.Linq;
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

            var winner = game.Player1.Count > 0 ? game.Player1 : game.Player2;
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
            return null;
        }
    }

    public class Game
    {        
        public List<int> Player1, Player2;

        public Game(List<int> player1, List<int> player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
    }
}
