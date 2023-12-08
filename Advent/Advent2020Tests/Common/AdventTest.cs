using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent2020Tests.Common
{
    public class AdventTest
    {
        public virtual string DataFile => "Data.txt";

        public virtual string[] GetLines()
        {
            // handle template specifically
            if (GetType() == typeof(Template.Day1)) return new string[0];
            // ReSharper disable once PossibleNullReferenceException
            string path = GetType().Namespace.Replace("Advent2020Tests.", "").Replace(".", "/");
            return File.ReadAllLines($"./{path}/{DataFile}");
        }

        protected void GiveAnswer(object value) => GiveAnswer(null, value);

        protected void GiveAnswer(object expected, object value)
        {
            if (value is long && expected is int i) expected = (long) i;
            if (expected == null || Equals(expected, value))
            {
                Console.WriteLine(value);
                return;
            }
            Console.WriteLine($"{value} - but correct answer is {expected}");
            Assert.AreEqual(expected, value);
        }
    }
}
