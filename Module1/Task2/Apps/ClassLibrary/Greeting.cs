using System;

namespace TimeGreeting
{
    public static class Greeting
    {
        public static string Greet(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "is cannot be empty.");
            }

            string currentTime = DateTime.Now.ToShortTimeString();

            return $"{currentTime} Hello, {name}!";
        }
    }
}
