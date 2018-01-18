namespace BashSoft.Output
{
    using System;
    using System.Collections.Generic;

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

        /// <summary>
        /// Print students data.
        /// </summary>
        /// <param name="students">Student for printing data.</param>
        public static void PrintStudent(KeyValuePair<string, double> students)
        {
            WriteMessageOnNewLine($"{students.Key} - {string.Join(", ", students.Value)}");
        }
    }
}