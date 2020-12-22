using System.Collections.Generic;
using System.Linq;
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
                string[] ingredients = parts[0].Split(' ').Select(s => s.Trim()).ToArray();
                string[] allergens = new string[0];
                if (parts.Length > 1)
                {
                    allergens = parts[1].Replace(")", "").Replace("contains ", "").Split(", ");
                }

                return new Food(ingredients, allergens);
            }).ToArray();
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer(1679, Problem1(GetData()));
        }

        private object Problem1(Food[] foods)
        {
            List<string> noAlergens = BuildNoAllergens(foods, out _);

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
            GiveAnswer("lmxt,rggkbpj,mxf,gpxmf,nmtzlj,dlkxsxg,fvqg,dxzq", Problem2(GetData()));
        }

        private object Problem2(Food[] foods)
        {
            BuildNoAllergens(foods, out var atoi);

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

        private static List<string> BuildNoAllergens(Food[] foods, out Dictionary<string, HashSet<string>> atoi)
        {
            var allI = new HashSet<string>();
            atoi = new Dictionary<string, HashSet<string>>();
            foreach (Food f in foods)
            {
                foreach (var a in f.Allergens)
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
                    if (!f.Allergens.Contains(a)) continue;
                    foreach (var i in atoi[a].ToArray())
                    {
                        if (!f.Ingredients.Contains(i)) atoi[a].Remove(i);
                    }
                }
            }

            var noAllergens = new List<string>();
            foreach (string i in allI)
            {
                if (!atoi.Values.Any(s => s.Contains(i)))
                {
                    noAllergens.Add(i);
                }
            }

            return noAllergens;
        }
    }

    public class Food
    {
        public string[] Ingredients;
        public string[] Allergens;

        public Food(string[] ingredients, string[] allergens)
        {
            Ingredients = ingredients;
            Allergens = allergens;
        }
    }
}
