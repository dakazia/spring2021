using System;
using TimeGreeting;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = Introduce();
            ShowGreeting(name);

            Console.ReadLine();
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

        private static void ShowGreeting(string name)
        {
            Console.WriteLine(Greeting.Greet(name));
        }
    }
}
