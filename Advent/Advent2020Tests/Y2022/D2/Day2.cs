using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Advent2020;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2022.D2
{
    [TestClass]
    public class Day2 : AdventTest
    {


      
        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(Problem2(GetLines()));
        }
        
        private object Problem2(string[] input)
        {
            int sum = 0;
            foreach (string line in input)
            {
                char opp = line[0]; // rock, paper, scissors
                char result = line[2]; // lose, draw, win
                char me = 'A';
                if (result == 'X')
                {
                    me = opp == 'A' ? 'C' : opp == 'B' ? 'A' : 'B';
                }
                if (result == 'Y')
                {
                    sum += 3;
                    me = opp == 'A' ? 'A' : opp == 'B' ? 'B' : 'C';
                }
                if (result == 'Z')
                {
                    sum += 6;
                    me = opp == 'A' ? 'B' : opp == 'B' ? 'C' : 'A';
                }

                sum += me == 'A' ? 1 : me == 'B' ? 2 : 3;
            }
            return sum;
        }
    }
}
