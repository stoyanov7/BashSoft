namespace BashSoft.Exceptions
{
    using System;

    public class InvalidNumberException : Exception
    {
        private const string NotValidNumber = "{0} is not a valid number.";

        public InvalidNumberException(string message)
            : base(string.Format(NotValidNumber, message))
        {
        }
    }
}