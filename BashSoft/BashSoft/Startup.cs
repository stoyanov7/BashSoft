namespace BashSoft
{
    using IO;
    using Judge;
    using Repositories;

    public class Startup
    {
        /// <summary>
        /// Entry point of program.
        /// </summary>
        /// <param studentByName="args">Arguments.</param>
        public static void Main(string[] args)
        {
            var tester = new Tester();
            var manager = new IoManager();
            var studentsRepository = new StudentsRepository(new RepositoryFilter(), new RepositorySorter());

            var currentInterpreter = new CommandInterpreter(tester, studentsRepository, manager);
            var reader = new InputReader(currentInterpreter);

            reader.StartReadingCommands();
        }
    }
}
