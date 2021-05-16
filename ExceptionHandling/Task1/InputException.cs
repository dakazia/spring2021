using System;

namespace Task1
{
    class InputException : ArgumentException
    {
        public InputException(string userInput)
        : base(userInput)
        {

        }
    }
}
