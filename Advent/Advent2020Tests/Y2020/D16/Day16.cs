using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventLibrary;

namespace Advent2020Tests.Y2020.D16
{
    [TestClass]
    public class Day16
    {
        public string[] GetLines()
        {
            return File.ReadAllLines("./Y2020/D16/Data.txt");
        }

        public Data GetData()
        {
            Ticket yt = null;
            List<Ticket> nearby = new List<Ticket>();
            Dictionary<string, Ranges> ranges = new Dictionary<string, Ranges>();
            bool onYour = false;
            bool onNearby = false;
            foreach (string line in GetLines())
            {
                if (line.Trim().Length > 0)
                {
                    

                    if (line.StartsWith("your ticket:"))
                    {
                        onYour = true;
                    }
                    else if (line.StartsWith("nearby tickets:"))
                    {
                        onYour = false;
                        onNearby = true;
                    }
                    else if (line.Trim().Length == 0) continue;
                    else if (line.Contains(":"))
                    {
                        string[] parts = line.Split(": ");
                        string[] rangeParts = parts[1].Split(" or ");
                        string[] lowRange = rangeParts[0].Split("-");
                        string[] highRange = rangeParts[1].Split("-");
                        ranges[parts[0]] = new Ranges(int.Parse(lowRange[0]), int.Parse(lowRange[1]),
                            int.Parse(highRange[0]), int.Parse(highRange[1]));
                    }
                    else
                    {
                        if (onYour) yt = ParseTicket(line);
                        else if (onNearby) nearby.Add(ParseTicket(line));
                        else throw new Exception("blah");
                    }
                }
            }

            return new Data(ranges, yt, nearby.ToArray());
        }

        [TestMethod]
        public void Problem1()
        {
            Console.WriteLine(Problem1(GetData()));
        }

        private object Problem1(Data data)
        {
            int sum = 0;
            foreach (var ticket in data.NearbyTickets)
            {
                foreach (int n in ticket.Numbers)
                {
                    bool found = false;
                    foreach (Ranges r in data.Classes.Values)
                    {
                        if (r.Contains(n))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        sum += n;
                    }
                }
            }

            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            Console.WriteLine(Problem2(GetData()));
        }

        private object Problem2(Data data)
        {
            List<Ticket> validTickets = data.NearbyTickets.Where(t => t.IsValid(data.Classes)).ToList();
            // fill a hashset with every field which is valid in each index
            List<HashSet<string>> possibleFields = new List<HashSet<string>>();
            foreach (int n in data.YourTicket.Numbers)
            {
                var fields = new HashSet<string>(data.Classes.Keys);
                possibleFields.Add(fields);
            }

            // for each valid ticket, if a number is outside the range of a given number, remove it from the set
            Parallel.For(0, data.YourTicket.Numbers.Length, i =>
            {
                foreach (string field in data.Classes.Keys)
                {
                    foreach (var ticket in validTickets)
                    {
                        if (!data.Classes[field].Contains(ticket.Numbers[i]))
                        {
                            possibleFields[i].Remove(field);
                            break;
                        }
                    }
                }
            });

            // go from known fields and eliminate that field from being other fields.
            foreach (var set in possibleFields.OrderBy(s => s.Count))
            {
                if (set.Count == 1)
                {
                    string val = set.First();
                    foreach (var s2 in possibleFields)
                    {
                        if (s2 != set)
                        {
                            s2.Remove(val);
                        }
                    }
                }
            }

            long product = 1;
            int index = 0;
            foreach (var fields in possibleFields)
            {
                if (fields.Count != 1) throw new Exception("doh");
                if (fields.First().StartsWith("departure"))
                {
                    product *= data.YourTicket.Numbers[index];
                }
                index++;
            }

            return product;
        }

        public Ticket ParseTicket(string line)
        {
            return new Ticket(line.Split(',').Select(s => int.Parse(s)).ToArray());
        }
    }

    public class Ranges
    {
        public int LowMin, LowMax, HighMin, HighMax;

        public Ranges(int lowMin, int lowMax, int highMin, int highMax)
        {
            LowMin = lowMin;
            LowMax = lowMax;
            HighMin = highMin;
            HighMax = highMax;
        }

        public bool Contains(int num)
        {
            return (LowMin <= num && num <= LowMax) || (HighMin <= num && num <= HighMax);
        }
    }

    public class Ticket
    {
        public int[] Numbers;

        public Ticket(int[] numbers)
        {
            Numbers = numbers;
        }

        public bool IsValid(Dictionary<string, Ranges> classes)
        {
            foreach (int n in Numbers)
            {
                bool found = false;
                foreach (Ranges r in classes.Values)
                {
                    if (r.Contains(n))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
    public class Data
    {
        public Dictionary<string, Ranges> Classes;
        public Ticket YourTicket;
        public Ticket[] NearbyTickets;

        public Data(Dictionary<string, Ranges> classes, Ticket yourTicket, Ticket[] nearbyTickets)
        {
            Classes = classes;
            YourTicket = yourTicket;
            NearbyTickets = nearbyTickets;
        }
    }
}
