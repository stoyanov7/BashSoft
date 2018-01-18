namespace BashSoft
{
    using Input;
    using Input.Contracts;
    using Judge;
    using Judge.Contracts;
    using Repositories;
    using Repositories.Contracts;

    public class Startup
    {
        /// <summary>
        /// Entry point of program.
        /// </summary>
        /// <param studentByName="args">Arguments.</param>
        public static void Main(string[] args)
        {
            IContentComparer tester = new Tester();
            IDirectoryManager manager = new IoManager();
            IDatabase studentsRepository = new StudentsRepository(new RepositoryFilter(), new RepositorySorter());

            ICommandInterpreter currentInterpreter =
                new CommandInterpreter(tester, studentsRepository, manager);

            var reader = new InputReader(currentInterpreter);

            reader.StartReadingCommands();
        }
    }
}
