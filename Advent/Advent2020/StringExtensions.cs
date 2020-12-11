using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventLibrary
{
    public static class StringExtensions
    {
        public static string[] Split(this string str, string val)
        {
            return str.Split(new string[] { val }, StringSplitOptions.None);
        }
    }
}
