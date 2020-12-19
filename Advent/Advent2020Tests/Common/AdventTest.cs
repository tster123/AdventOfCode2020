using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Common
{
    public class AdventTest
    {
        public virtual string[] GetLines()
        {
            // handle template specifically
            if (GetType() == typeof(Days.ATemplate.Day)) return new string[0];
            // ReSharper disable once PossibleNullReferenceException
            string path = GetType().Namespace.Replace("Advent2020Tests.", "").Replace(".", "/");
            return File.ReadAllLines($"./{path}/Data.txt");
        }

        protected void GiveAnswer(object value) => GiveAnswer(null, value);

        protected void GiveAnswer(object expected, object value)
        {
            if (expected == null || expected == value)
            {
                Console.WriteLine(value);
                return;
            }
            Console.WriteLine($"{value} - but correct answer is {expected}");
            Assert.AreEqual(expected, value);
        }
    }
}
