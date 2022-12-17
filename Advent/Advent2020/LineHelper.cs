using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020
{
    public static class LineHelper
    {
        /// <summary>
        /// Creates groups from lines where the groups are separated by empty lines.
        /// </summary>
        public static List<List<string>> Groupify(this IEnumerable<string> lines)
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
