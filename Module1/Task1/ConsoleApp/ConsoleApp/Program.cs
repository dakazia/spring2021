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
            string name;
            do
            {
                Console.WriteLine("Please, introduce yourself:");
                name = Console.ReadLine();

            } while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name));

            return name;
        }

        private static void Greet(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
    }
}
