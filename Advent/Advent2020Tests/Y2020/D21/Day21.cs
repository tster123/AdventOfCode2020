using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Advent2020Tests.Common;
using AdventLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Y2020.D21
{
    [TestClass]
    public class Day21 : AdventTest
    {

        public Food[] GetData()
        {
            return GetLines().Select(l =>
            {
                string[] parts = l.Split(" (");
                string[] ingr = parts[0].Split(' ').Select(s => s.Trim()).ToArray();
                string[] alergens = new string[0];
                if (parts.Length > 1)
                {
                    alergens = parts[1].Replace(")", "").Replace("contains ", "").Split(", ");
                }

                return new Food(ingr, alergens);
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(1679, Problem1(GetData()));
        }

        private object Problem1(Food[] foods)
        {
            var allI = new HashSet<string>();
            var atoi = new Dictionary<string, HashSet<string>>();
            foreach (Food f in foods)
            {
                foreach (var a in f.Alergens)
                {
                    if (!atoi.ContainsKey(a)) atoi[a] = new HashSet<string>();
                    foreach (var i in f.Ingredients)
                    {
                        allI.Add(i);
                        atoi[a].Add(i);
                    }
                }
            }

            foreach (Food f in foods)
            {
                foreach (var a in atoi.Keys)
                {
                    if (!f.Alergens.Contains(a)) continue;
                    foreach (var i in atoi[a].ToArray())
                    {
                        if (!f.Ingredients.Contains(i)) atoi[a].Remove(i);
                    }
                }
            }

            var noAlergens = new List<string>();
            foreach (string i in allI)
            {
                if (!atoi.Values.Any(s => s.Contains(i)))
                {
                    noAlergens.Add(i);
                }
            }

            int sum = 0;
            foreach (var i in noAlergens)
            {
                foreach (var food in foods)
                {
                    if (food.Ingredients.Contains(i)) sum++;
                }
            }
            return sum;
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Food[] foods)
        {
            var allI = new HashSet<string>();
            var atoi = new Dictionary<string, HashSet<string>>();
            foreach (Food f in foods)
            {
                foreach (var a in f.Alergens)
                {
                    if (!atoi.ContainsKey(a)) atoi[a] = new HashSet<string>();
                    foreach (var i in f.Ingredients)
                    {
                        allI.Add(i);
                        atoi[a].Add(i);
                    }
                }
            }

            foreach (Food f in foods)
            {
                foreach (var a in atoi.Keys)
                {
                    if (!f.Alergens.Contains(a)) continue;
                    foreach (var i in atoi[a].ToArray())
                    {
                        if (!f.Ingredients.Contains(i)) atoi[a].Remove(i);
                    }
                }
            }

            var noAlergens = new List<string>();
            foreach (string i in allI)
            {
                if (!atoi.Values.Any(s => s.Contains(i)))
                {
                    noAlergens.Add(i);
                }
            }

            bool didSomething = true;
            var itoa = new Dictionary<string, string>();
            while (didSomething)
            {
                didSomething = false;
                foreach (var a in atoi.Keys)
                {
                    if (atoi[a].Count == 1)
                    {
                        string i = atoi[a].Single();
                        itoa[i] = a;
                        foreach (var a2 in atoi.Keys)
                        {
                            if (a2 != a)
                            {
                                if (atoi[a2].Remove(i)) didSomething = true;
                            }
                        }
                    }
                }
            }

            return string.Join(",", atoi.Values.SelectMany(s => s).OrderBy(i => itoa[i]));
        }
    }

    public class Food
    {
        public string[] Ingredients;
        public string[] Alergens;

        public Food(string[] ingredients, string[] alergens)
        {
            Ingredients = ingredients;
            Alergens = alergens;
        }
    }
}
