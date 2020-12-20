using System;
using System.Collections.Generic;
using System.Linq;
using Advent2020;
using Advent2020Tests.Common;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2020.D20
{

    public enum TileType
    {
        Unknown, Corner, Edge, Interior
    }
    public class Tile : DimensionMap<char>
    {
        public readonly int Id;

        public List<string> Edges = new List<string>();
        public TileType Type { get; set; } = TileType.Unknown;
        private int Size { get; }
        private int MaxIndex => Size - 1;
        public Tile(int id, int size, int dimensions, [NotNull] IEnumerable<Point<char>> points, char defaultValue = default, bool isInfinite = true, bool wraps = false) 
            : base(dimensions, points, defaultValue, isInfinite, wraps)
        {
            Id = id;
            Size = size;
            string n = "", s = "", e = "", w = "";
            for (int i = 0; i < size; i++)
            {

                n += this[new Point<char>(new[] {0, i})];
                e += this[new Point<char>(new[] {i, MaxIndex})];
                s += this[new Point<char>(new[] {MaxIndex, MaxIndex - i})];
                w += this[new Point<char>(new[] {MaxIndex - i, 0})];
            }

            Edges.AddRange(new[] {n, e, s, w});
        }

        public static Tile MakeTile(int id, IEnumerable<string> lines)
        {
            List<Point<char>> points = new List<Point<char>>();
            int row = 0;
            int size = 0;
            foreach (string line in lines)
            {
                size = line.Length;
                for (int col = 0; col < line.Length; col++)
                {
                    points.Add(new Point2D<char>(new[] { col, row }, line[col]));
                }

                row++;
            }
            return new Tile(id, size, 2, points);
        }

        public Tile RotateCounterClockwise()
        {
            List<Point<char>> points = new List<Point<char>>();
            foreach (var p in Points)
            {
                points.Add(new Point2D<char>(new[] {p[1], MaxIndex - p[0]}, p.Value));
            }

            return new Tile(Id, Size, 2, points) {Type = Type};
        }

        public Tile FlipAlongVertical()
        {
            List<Point<char>> points = new List<Point<char>>();
            foreach (var p in Points)
            {
                points.Add(new Point2D<char>(new[] { p[0], MaxIndex - p[1] }, p.Value));
            }

            return new Tile(Id, Size, 2, points) { Type = Type };
        }

        public Tile FlipAlongHorizontal()
        {
            List<Point<char>> points = new List<Point<char>>();
            foreach (var p in Points)
            {
                points.Add(new Point2D<char>(new[] { MaxIndex - p[0], p[1] }, p.Value));
            }

            return new Tile(Id, Size, 2, points) { Type = Type };
        }

        public bool Matches(string edge)
        {
            string rev = string.Concat(edge.Reverse());
            foreach (string e in Edges)
            {
                if (e == edge || e == rev) return true;
            }

            return false;
        }

        private bool EdgesMatch(string a, string b)
        {
            if (a == b) return true;
            if (a == string.Concat(b.Reverse())) return true;
            return false;
        }

        public Tile SpinAndFlipToMatchThisWestToLeftsEast(string leftsEast)
        {
            int save = 0;
            Tile toRet = this;
            while (true)
            {
                if (EdgesMatch(toRet.Edges[3], leftsEast))
                {
                    break;
                }

                toRet = toRet.RotateCounterClockwise();
                if (save++ > 5) throw new Exception("oops");
            }

            if (toRet.Edges[3] == leftsEast) return toRet.FlipAlongHorizontal();
            return toRet;
        }

        public Tile SpinAndFlipToMatchThisNorthToAbovesSouth(string abovesSouth)
        {
            int save = 0;
            Tile toRet = this;
            while (true)
            {
                if (EdgesMatch(toRet.Edges[0], abovesSouth))
                {
                    break;
                }

                toRet = toRet.RotateCounterClockwise();
                if (save++ > 5) throw new Exception("oops");
            }

            if (toRet.Edges[0] == abovesSouth) return toRet.FlipAlongVertical();
            return toRet;
        }
    }

    [TestClass]
    public class Day20 : AdventTest
    {
        //public override string DataFile => "Test.txt";

        public List<Tile> GetData()
        {
            var ret = new List<Tile>();
            int currentTile = -1;
            List<string> tileLines = new List<string>();
            foreach (string line in GetLines())
            {
                if (line == "")
                {
                    ret.Add(Tile.MakeTile(currentTile, tileLines));
                    tileLines = new List<string>();
                    currentTile = -1;
                }
                else if (line.StartsWith("Tile "))
                {
                    currentTile = int.Parse(line.Replace("Tile ", "").Replace(":", ""));
                }
                else
                {
                    tileLines.Add(line);
                }
            }

            if (currentTile != -1)
            {
                ret.Add(Tile.MakeTile(currentTile, tileLines));
            }
            return ret;
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(Problem1(GetData()));
        }

        private object Problem1(List<Tile> tiles)
        {
            Dictionary<string, int> numSeen = new Dictionary<string, int>();
            foreach (var tile in tiles)
            {
                foreach (string edge in tile.Edges)
                {
                    string rev = string.Concat(edge.Reverse());
                    string smaller = string.Compare(rev, edge, StringComparison.Ordinal) < 0 ? rev : edge;
                    if (!numSeen.ContainsKey(smaller))
                    {
                        numSeen[smaller] = 0;
                    }

                    numSeen[smaller]++;
                }
            }

            long product = 1;
            foreach (var tile in tiles)
            {
                int numSingle = 0;
                foreach (string edge in tile.Edges)
                {
                    string rev = string.Concat(edge.Reverse());
                    string smaller = string.Compare(rev, edge, StringComparison.Ordinal) < 0 ? rev : edge;
                    if (numSeen[smaller] == 1) numSingle++;
                }

                if (numSingle >= 2)
                {
                    Console.WriteLine(tile.Id);
                    product *= tile.Id;
                }
            }
            return product;
        }

        private string[] SeaMonster = new[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(1639, Problem2(GetData()));
        }

        // 144 tiles, it's a 12x12
        private object Problem2(List<Tile> tiles)
        {
            var (corners, edges, interior) = SeparateEdges(tiles);
            var placement = DoPlacement(tiles, corners);

            List<Point<char>> field = new List<Point<char>>();
            foreach (var tilePoint in placement.Keys)
            {
                Tile t = placement[tilePoint];
                foreach (var p in t.Points)
                {
                    // exclude borders
                    if (p[0] == 0 || p[1] == 0 || p[0] == 9 || p[1] == 9) continue;
                    field.Add(new Point<char>(new[] {tilePoint[0] * 8 + p[0] - 1, tilePoint[1] * 8 + p[1] - 1}, p.Value));
                }
            }

            var map = new Tile(0, 96, 2, field, defaultValue: '~');

            int numMonsters = 0;
            int tries = 0;
            while (true)
            {
                numMonsters = CountSeaMonsters(map);
                if (numMonsters > 0) break;

                map = map.FlipAlongVertical();
                numMonsters = CountSeaMonsters(map);
                if (numMonsters > 0) break;

                map = map.FlipAlongHorizontal();
                numMonsters = CountSeaMonsters(map);
                if (numMonsters > 0) break;

                map = map.FlipAlongVertical();
                numMonsters = CountSeaMonsters(map);
                if (numMonsters > 0) break;

                map = map.FlipAlongHorizontal();
                map = map.RotateCounterClockwise();

                tries++;
                if (tries > 5) throw new Exception(":!");
            }

            var seaMonster = MapFactories.Character2D(SeaMonster);
            var allPoints = new Dictionary<Point<char>, Point<char>>();
            
            // uncomment this to print out the monsters highlighted.
            foreach (var p in map.Points) allPoints[p] = p;
            foreach (var monster in GetSeaMonsters(map))
            {
                foreach (var mp in seaMonster.Points.Where(m => m.Value == '#'))
                {
                    var target = monster.ApplyVector(mp.Vector);
                    if (allPoints[target].Value != '#') throw new Exception("SDOSD");
                    allPoints[target] = new Point<char>(target.Vector, '0');
                }
            }

            var db = new DimensionMap<char>(2, allPoints.Values);
            Console.WriteLine();
            Console.WriteLine(db);

            
            var totalHashes = map.Points.Where(p => p.Value == '#').Count();
            int seaMonsterSize = seaMonster.Points.Where(p => p.Value == '#').Count();
            return totalHashes - numMonsters * seaMonsterSize;
        }

        private int CountSeaMonsters(Tile map) => GetSeaMonsters(map).Count;

        private List<Point<char>> GetSeaMonsters(Tile map)
        {
            var list = new List<Point<char>>();
            var seaMonster = MapFactories.Character2D(SeaMonster);
            int numFound = 0;
            for (int x = 0; x <= 96; x++)
            for (int y = 0; y <= 96; y++)
            {
                if (IsSeaMonster(map, seaMonster, x, y))
                {
                    list.Add(new Point<char>(new[] {y, x}));
                    Console.WriteLine($"Found Monster at {x},{y}");
                    numFound++;
                }
            }

            return list;
        }

        private bool IsSeaMonster(Tile map, DimensionMap<char> monster, int x, int y)
        {
            foreach (var p in monster.Points.Where(p => p.Value == '#'))
            {
                if (map[new Point<char>(new[] {y + p[0], x + p[1]})] != '#') return false;
            }

            return true;
        }

        private static Dictionary<Point<bool>, Tile> DoPlacement(List<Tile> tiles, List<Tile> corners)
        {
            Dictionary<Point<bool>, Tile> placement = new Dictionary<Point<bool>, Tile>();
            placement[new Point2D<bool>(0, 0, true)] = corners[0];
            tiles.Remove(corners[0]);
            corners.RemoveAt(0);
            // manually debugged and found that 0,10 is a corner, hence it is 11 wide 13 tall.
            for (int i = 1; i < 12; i++)
            {
                // place tile to the right
                Tile prevLeft = placement[new Point2D<bool>(0, i - 1, true)];
                bool noMatchLeft = true;
                foreach (var t in tiles.ToArray())
                {
                    if (t.Matches(prevLeft.Edges[1]))
                    {
                        if (!noMatchLeft) throw new Exception("eeek");
                        Tile rotated = t.SpinAndFlipToMatchThisWestToLeftsEast(prevLeft.Edges[1]);
                        if (rotated.Edges[3] != string.Concat(prevLeft.Edges[1].Reverse()))
                            throw new Exception("what?");
                        placement[new Point2D<bool>(0, i, true)] = rotated;
                        tiles.Remove(t);
                        noMatchLeft = false;
                    }
                }

                if (noMatchLeft) throw new Exception(":(");
            }

            for (int x = 0; x < 12; x++)
            {
                // place tiles downwards
                for (int y = 1; y < 12; y++)
                {
                    Tile prevUp = placement[new Point2D<bool>(y - 1, x, true)];
                    bool noMatch = true;
                    foreach (var t in tiles.ToArray())
                    {
                        if (t.Matches(prevUp.Edges[2]))
                        {
                            if (!noMatch) throw new Exception("eeek");
                            Tile rotated = t.SpinAndFlipToMatchThisNorthToAbovesSouth(prevUp.Edges[2]);
                            if (rotated.Edges[0] != string.Concat(prevUp.Edges[2].Reverse()))
                                throw new Exception("what?");
                            placement[new Point2D<bool>(y, x, true)] = rotated;
                            tiles.Remove(t);
                            noMatch = false;
                        }
                    }

                    if (noMatch) throw new Exception(":(");
                }
            }

            return placement;
        }

        private IEnumerable<Tile> GetCandidatesForSpot(Point<bool> nextPlace, List<Tile> corners, List<Tile> edges, List<Tile> interior)
        {
            int x = nextPlace[0];
            int y = nextPlace[1];
            if (x == 0 && y == 12)
            {
                return corners;
            }
            else if (x == 0 && y == 10)
            {
                return edges.Concat(corners);
            }
            else if (x == 10 && (y == 0 || y == 12))
            {
                return corners;
            }
            else if (x == 12 && (y == 10 || y == 0))
            {
                return corners;
            }
            else if (x == 0 || y == 0 || x == 12 || y == 12)
            {
                return edges;
            }
            else if (x == 10 || y == 10)
            {
                return edges.Concat(interior);
            }
            else
            {
                return interior;
            }
        }

        private Tuple<List<Tile>, List<Tile>, List<Tile>> SeparateEdges(List<Tile> tiles)
        {
            Dictionary<string, int> numSeen = new Dictionary<string, int>();
            foreach (var tile in tiles)
            {
                foreach (string edge in tile.Edges)
                {
                    string rev = string.Concat(edge.Reverse());
                    if (!numSeen.ContainsKey(edge))
                    {
                        numSeen[edge] = 0;
                    }
                    if (!numSeen.ContainsKey(rev))
                    {
                        numSeen[rev] = 0;
                    }

                    numSeen[edge]++;
                    numSeen[rev]++;
                }
            }

            var corners = new List<Tile>();
            var edges = new List<Tile>();
            var interior = new List<Tile>();

            for (var index = 0; index < tiles.Count; index++)
            {
                var tile = tiles[index];
                int numSingle = 0;
                bool[] matched = new bool[4];
                int i = 0;
                foreach (string edge in tile.Edges)
                {
                    if (numSeen[edge] == 1)
                    {
                        matched[i] = false;
                        numSingle++;
                    }
                    else matched[i] = true;

                    i++;
                }

                if (numSingle >= 2)
                {
                    while (numSeen[tile.Edges[3]] > 1 || numSeen[tile.Edges[0]] > 1)
                    {
                        tile = tile.RotateCounterClockwise();
                    }
                    tile.Type = TileType.Corner;
                    corners.Add(tile);
                    tiles[index] = tile; // update the tile list
                }
                else if (numSingle == 1)
                {
                    tile.Type = TileType.Edge;
                    edges.Add(tile);
                }
                else
                {
                    tile.Type = TileType.Interior;
                    interior.Add(tile);
                }
            }

            return new Tuple<List<Tile>, List<Tile>, List<Tile>>(corners, edges, interior);
        }
    }
}
