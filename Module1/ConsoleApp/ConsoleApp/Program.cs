using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = Introduce();
            Greet(name);
        }

        private static string Introduce()
        {
            Console.WriteLine("Please, introduce yourself:");
            return Console.ReadLine();
        }

        private static void Greet(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
    }
}
