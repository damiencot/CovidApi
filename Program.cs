using System;

namespace CovidApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            while (true)
            {
                args = Console.ReadLine().Split(' ');
                Console.WriteLine(args[0]);
            }
        }
    }
}
