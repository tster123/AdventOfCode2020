using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2020.D11
{
    [TestClass]
    public class Day11
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D11/Data.txt");
        }

        public Room GetData()
        {
            return new Room(GetLines().Select(l =>
            {
                bool[] seats = new bool[l.Length];
                for (int i = 0; i < l.Length; i++)
                {
                    if (l[i] == 'L') seats[i] = true;
                }

                return new Row(seats, new bool[seats.Length]);
            }).ToArray());
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(Room room)
        {
            HashSet<string> seen = new HashSet<string>();
            while (!seen.Contains(room.ToString()))
            {
                seen.Add(room.ToString());
                room.Tick();
            }

            return room.CountOccupied();
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(Room room)
        {
            HashSet<string> seen = new HashSet<string>();
            while (!seen.Contains(room.ToString()))
            {
                seen.Add(room.ToString());
                room.Tick2();
            }

            return room.CountOccupied();
        }
    }

    public class Room
    {
        public Row[] Rows;

        public Room(Row[] rows)
        {
            Rows = rows;
        }

        public int CountOccupied()
        {
            return Rows.Sum(r => r.Occupied.Count(a => a));
        }

        public override string ToString()
        {
            return string.Join("\n", Rows.Select(r => r.ToString()));
        }

        public void Tick()
        {
            Row[] newRows = new Row[Rows.Length];
            int i = 0;
            foreach (Row row in Rows)
            {
                Row lastRow = i == 0 ? null : Rows[i - 1];
                Row nextRow = i == Rows.Length - 1 ? null : Rows[i + 1];
                newRows[i] = row.Tick(lastRow, nextRow);
                i++;
            }

            Rows = newRows;
        }

        public void Tick2()
        {
            Row[] newRows = new Row[Rows.Length];
            int i = 0;
            foreach (Row row in Rows)
            {
                newRows[i] = row.Tick2(i, Rows);
                i++;
            }

            Rows = newRows;
        }
    }

    public class Row
    {
        public bool[] Seats;
        public bool[] Occupied;

        public Row(bool[] seats, bool[] occupied)
        {
            Seats = seats;
            Occupied = occupied;
        }

        public override string ToString()
        {
            String s = "";
            for (int i = 0; i < Seats.Length; i++)
            {
                if (Occupied[i]) s += "#";
                else if (Seats[i]) s += "L";
                else s += ".";
            }

            return s;
        }

        public Row Tick2(int row, Row[] rows)
        {
            bool[] newOccupied = new bool[Occupied.Length];

            for (int i = 0; i < Occupied.Length; i++)
            {
                if (!Seats[i]) continue;
                int numAdjacent = 0;
                
                // up-left
                int cRow = row - 1;
                int cCol = i - 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cRow--;
                        cCol--;
                        continue;
                    }
                    
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                // left
                cRow = row;
                cCol = i - 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cCol--;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                        
                    }

                    break;
                }

                // down-left
                cRow = row + 1;
                cCol = i - 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cCol--;
                        cRow++;
                        continue;
                    }

                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                        
                    }

                    break;
                }

                // up-right
                cRow = row - 1;
                cCol = i + 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cRow--;
                        cCol++;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                // right
                cRow = row;
                cCol = i + 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cCol++;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                // down-right
                cRow = row + 1;
                cCol = i + 1;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cCol++;
                        cRow++;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                // down
                cRow = row + 1;
                cCol = i;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cRow++;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                // up
                cRow = row - 1;
                cCol = i;
                while (cRow >= 0 && cCol >= 0 && cRow < rows.Length && cCol < Occupied.Length)
                {
                    if (!rows[cRow].Seats[cCol])
                    {
                        cRow--;
                        continue;
                    }
                    if (rows[cRow].Occupied[cCol])
                    {
                        numAdjacent++;
                    }

                    break;
                }

                if (numAdjacent == 0)
                {
                    newOccupied[i] = true;
                }
                else if (numAdjacent >= 5)
                {
                    newOccupied[i] = false;
                }
                else
                {
                    newOccupied[i] = Occupied[i];
                }
            }

            return new Row(Seats, newOccupied);
        }

        public Row Tick(Row a, Row b)
        {
            if (a == null)
            {
                a = b;
                b = null;
            }

            bool[] newOccupied = new bool[a.Occupied.Length];

            for (int i = 0; i < a.Occupied.Length; i++)
            {
                if (!Seats[i]) continue;
                int numAdjacent = 0;
                if (i > 0)
                {
                    if (a.Occupied[i - 1]) numAdjacent++;
                    if (Occupied[i - 1]) numAdjacent++;
                    if (b?.Occupied[i - 1] ?? false) numAdjacent++;
                }
                if (a.Occupied[i]) numAdjacent++;
                if (b?.Occupied[i] ?? false) numAdjacent++;
                if (i < a.Occupied.Length - 1)
                {
                    if (a.Occupied[i + 1]) numAdjacent++;
                    if (Occupied[i + 1]) numAdjacent++;
                    if (b?.Occupied[i + 1] ?? false) numAdjacent++;
                }

                if (numAdjacent == 0)
                {
                    newOccupied[i] = true;
                }
                else if (numAdjacent >= 4)
                {
                    newOccupied[i] = false;
                }
                else
                {
                    newOccupied[i] = Occupied[i];
                }
            }

            return new Row(Seats, newOccupied);
        }
    }
}
