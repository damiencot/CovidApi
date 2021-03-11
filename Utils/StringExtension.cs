using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApi.Utils
{
    public static class StringExtension
    {

        public static string toTitleCase(this string str)
        {
            str = str.Replace("\"", "").ToLower();
            str = char.ToUpper(str[0]) + str.Substring(1);
            return str;

        }
    }
}
