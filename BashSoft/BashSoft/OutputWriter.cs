namespace BashSoft
{
    using System;

    public static class OutputWriter
    {
        /// <summary>
        /// Write a message on single line.
        /// </summary>
        /// <param name="message">Message for writing.</param>
        public static void WriteMessage(string message) => Console.Write(message);

        /// <summary>
        /// Write a message on new line.
        /// </summary>
        /// <param name="message">Message for writing.</param>
        public static void WriteMessageOnNewLine(string message) => Console.WriteLine(message);

        /// <summary>
        /// Write a new empty line.
        /// </summary>
        public static void WriteEmptyLine() => Console.WriteLine();

        /// <summary>
        /// Write a different kind of message which is an error/exception.
        /// </summary>
        /// <param name="message">Message for writing.</param>
        public static void DisplayException(string message)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
    }
}