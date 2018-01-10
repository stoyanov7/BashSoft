namespace BashSoft
{
    public class Startup
    {
        /// <summary>
        /// Entry point of program.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            IoManager.TraverseDirectory(@"C:\Users\Joro\Desktop");
        }
    }
}
