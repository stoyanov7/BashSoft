namespace BashSoft
{
    using IO;

    public class Startup
    {
        /// <summary>
        /// Entry point of program.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            InputReader.StartReadingCommands();
        }
    }
}
