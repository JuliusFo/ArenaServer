using System;

namespace ArenaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string typed = "";
            while(typed != "Exit")
            {
                typed = Console.ReadLine();
            }
        }
    }
}
