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

        public static long ToLong(this string str, int radix)
        {
            return Convert.ToInt64(str, radix);
        }

        public static int ToInt(this string str, int radix)
        {
            return Convert.ToInt32(str, radix);
        }
    }

    public static class IntExtensions
    {
        public static string ToStringWithBase(this int v, int radix)
        {
            return Convert.ToString(v, radix);
        }

        public static string ToStringWithBase(this long v, int radix)
        {
            return Convert.ToString(v, radix);
        }
    }
}
