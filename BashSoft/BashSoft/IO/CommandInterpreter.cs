namespace BashSoft.IO
{
    using System;
    using System.Linq;
    using Bytes2you.Validation;
    using Commands;
    using Commands.Contracts;
    using Contracts;
    using Exceptions;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly IContentComparer tester;
        private readonly IDatabase studentsRepository;
        private readonly IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer tester, IDatabase studentsRepository, IDirectoryManager inputOutputManager)
        {
            this.tester = tester;
            this.studentsRepository = studentsRepository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            Guard.WhenArgument(input, "input").IsNullOrEmpty().Throw();

            var data = input
                .Split(' ')
                .ToArray();

            var commandName = data[0].ToLower();
                
            try
            {
                var command = this.ParseCommand(input, commandName, data);
                command.Execute();
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private IExecutable ParseCommand(string input, string command, string[] data)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "mkdir":
                    return new MakeDirectoryCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "ls":
                    return new TraverseFoldersCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "cmp":
                    return new CompareFilesCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "cdrel":
                    return new ChangeRelativePathCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "cdabs":
                    return new ChangeAbsolutePathCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "readdb":
                    return new ReadDatabaseCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "show":
                    return new ShowCourseCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "filter":
                    return new PrintFilteredStudentsCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "order":
                    return new PrintOrderedStudentsCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "dropdb:":
                    return new DropDatabaseCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                case "help":
                    return new GetHelpCommand(input, data, this.tester, this.studentsRepository, this.inputOutputManager);
                default:
                    throw new InvalidCommandException(input);
            }
        }
    }
}