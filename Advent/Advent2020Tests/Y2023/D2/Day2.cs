using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2023.D2
{
    [TestClass]
    public class Day2 : AdventTest
    {

        public class Game
        {
            public int Id;
            public Show[] Shows;
        }

        public class Show
        {
            public int Red, Green, Blue;
        }

        public Game[] GetData()
        {
            return GetLines().Select(l =>
            {
                string[] parts = l.Split(":");
                int id = int.Parse(parts[0].Replace("Game ", ""));
                var colors = parts[1].Split(",").Select(p => p.Trim());
                List<Show> shows = new();
                foreach (string showStr in parts[1].Split(";"))
                {
                    Show show = new Show();
                    foreach (var c in showStr.Split(",").Select(p => p.Trim()))
                    {
                        string[] mork = c.Split(" ");
                        if (mork[1] == "red") show.Red = int.Parse(mork[0]);
                        if (mork[1] == "green") show.Green = int.Parse(mork[0]);
                        if (mork[1] == "blue") show.Blue = int.Parse(mork[0]);
                    }

                    shows.Add(show);
                }

                return new Game { Id = id, Shows = shows.ToArray() };
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(Game[] lines)
        {
            int sum = 0;
            foreach (Game line in lines)
            {
                if (line.Shows.Any(s => s.Red > 12 || s.Green > 13 || s.Blue > 14))
                {
                    //Console.WriteLine("excluding " + line.Id);
                    continue;

                }

                // Console.WriteLine("Including " + line.Id);
                sum += line.Id;
            }
            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Game[] games)
        {
            int sum = 0;
            foreach (Game game in games)
            {
                int power = game.Shows.Max(g => g.Blue) *
                            game.Shows.Max(g => g.Green) *
                            game.Shows.Max(g => g.Red);
                sum += power;
            }
            return sum;
        }
    }
}
