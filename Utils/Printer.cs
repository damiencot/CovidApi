using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApi.Utils
{
    public class Printer
    {
        public int TableWidth { get; set; }

        public void Line()
        {
            Console.WriteLine(new string('-', TableWidth));
        }

        public void Row(params string[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCenter(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        public string AlignCenter(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}