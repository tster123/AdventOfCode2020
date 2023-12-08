using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Template
{
    [TestClass]
    public class Day1 : AdventTest
    {

        public object[] GetData()
        {
            return GetLines().Select(l =>
            {
                return (object) null;
            }).ToArray();
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
}
