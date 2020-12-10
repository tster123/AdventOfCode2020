using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020
{
    public class LineHelper
    {
        public static List<List<string>> Groupify(IEnumerable<string> lines)
        {
            var ret = new List<List<string>>();
            var currentGroup = new List<string>();
            foreach (var line in lines)
            {
                if (line == "")
                {
                    ret.Add(currentGroup);
                    currentGroup = new List<string>();
                }
                else
                {
                    currentGroup.Add(line);
                }
            }

            ret.Add(currentGroup);
            return ret;
        }
    }
}
