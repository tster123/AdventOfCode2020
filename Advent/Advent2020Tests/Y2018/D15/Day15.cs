using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2020;
using Advent2020Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
namespace Advent2020Tests.Y2018.D15
{
    [TestClass]
    public class Day15 : AdventTest
    {

        public Game GetData()
        {
            var points = MapFactories.Character2D(GetLines()).Points;
            return new Game();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(object[] lines)
        {
            return null;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(object[] lines)
        {
            return null;
        }
    }

    public class Game
    {
        public IBoardThing[,] Board;
        private int Rows;
        private int Cols;

        public Game(DimensionMap<char> state)
        {
            Board = new IBoardThing[state.GetRange(0).Max + 1, state.GetRange(1).Max + 1];
            foreach (var p in state.Points)
            {
                IBoardThing t;
                if (p.Value == '.') t = new Floor(p);
                else if (p.Value == '#') t = new Wall(p);
                else
                {
                    t = new Critter(p.Value, p);
                    (p.Value == 'G' ? Goblins : Elves).Add((Critter)t);
                }
                Board[p[0], p[1]] = t;
            }
        }

        public bool GameIsOver = false;
        public List<Critter> Goblins = new List<Critter>();
        public List<Critter> Elves = new List<Critter>();
        public IBoardThing this[Point<char> p] => Board[p[0], p[1]];

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Board.Length + Board.GetLength(0));

        }
    }

    public interface IBoardThing
    {
        char Char { get; }
        void DoTurn(Game game);
        Point<char> Location { get; }
    }

    public abstract class BoardThing : IBoardThing
    {
        public Point<char> Location { get; set; }

        protected BoardThing(Point<char> location)
        {
            Location = location;
        }

        public abstract char Char { get; }

        public virtual void DoTurn(Game game)
        {
        }
    }

    public class Floor : BoardThing
    {
        public char Char => '.';

        public Floor(Point<char> location) : base(location)
        {
        }
    }

    public class Wall : BoardThing
    {
        public char Char => '#';

        public Wall(Point<char> location) : base(location)
        {
        }
    }

    // goblin or elf
    public class Critter : BoardThing
    {
        public char Char { get; }

        public int HitPoints = 200;
        public int AttackPower = 3;

        public bool IsGoblin => Char == 'G';
        public char EnemyChar => IsGoblin ? 'E' : 'G';

        public Critter(char c, Point<char> location) : base(location)
        {
            Char = c;
        }

        public override void DoTurn(Game game)
        {
            // dead things don't go.
            if (HitPoints <= 0) return;

            List<Critter> enemies = IsGoblin ? game.Elves : game.Goblins;
            if (enemies.Count == 0)
            {
                game.GameIsOver = true;
                return;
            }

            DoMove(game);
        }

        private void DoMove(Game game)
        {
            // check if I am already in range of a target.
            foreach (var p in Location.GetNeighbors(false))
            {
                IBoardThing nextTo = game[p];
                if (nextTo.Char == EnemyChar) return;
            }
        }

        private IEnumerable<IBoardThing> GetNearestDestinations(Game game)
        {
            // breadth first search
            Queue<Point<char>> queue = new Queue<Point<char>>();
            queue.Enqueue(Location);
            HashSet<Point<char>> seen = new HashSet<Point<char>>();
            seen.Add(Location);
            List<IBoardThing> ret = new List<IBoardThing>();
            while (true)
            {
                if (queue.Count == 0) return new IBoardThing[0]; // no valid paths

                // have to do a whole level at a time because they are equally near
                for (int i = 0; i < queue.Count; i++)
                {
                    var p = queue.Dequeue();
                    foreach (var n in p.GetNeighbors(false))
                    {
                        if (seen.Contains(n)) continue;
                        IBoardThing atN = game[n];
                        if (atN.Char == '.')
                        {
                            seen.Add(n);
                            queue.Enqueue(n);
                            if (IsAdjacentToEnemy(n, game)) ret.Add(atN);
                        }
                    }
                }
                if (ret.Count > 0) return ret;
            }
        }

        private bool IsAdjacentToEnemy(Point<char> p, Game game) =>
            p.GetNeighbors(false).Any(p2 => game[p2].Char == EnemyChar);

        internal static Point<char> GetMinByReadingOrder(IEnumerable<Point<char>> points)
        {
            Point<char> min = null;
            foreach (var p in points)
            {
                if (min == null) min = p;
                else
                {
                    if (min[0] > p[0] || (min[0] == p[0] && min[1] > p[1]))
                    {
                        min = p;
                    }
                }
            }

            if (min == null) throw new InvalidOperationException("unexpected null");
            return min;
        }
    }

    [TestClass]
    public class CritterTest
    {
    }
}
*/