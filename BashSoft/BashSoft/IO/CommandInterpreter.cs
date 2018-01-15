namespace BashSoft.IO
{
    using System.Diagnostics;
    using System.Linq;
    using Exceptions;
    using Judge;
    using Repositories;
    using StaticData;

    public class CommandInterpreter
    {
        private readonly Tester tester;
        private readonly StudentsRepository studentsRepository;
        private readonly IoManager inputOutputManager;

        public CommandInterpreter(Tester tester, StudentsRepository studentsRepository, IoManager inputOutputManager)
        {
            this.tester = tester;
            this.studentsRepository = studentsRepository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            var data = input
                .Split(' ')
                .ToArray();

            var command = data[0];

            switch (command)
            {
                case "open":
                    this.TryOpenFile(input, data);
                    break;
                case "mkdir":
                    this.TryCreateDirectory(input, data);
                    break;
                case "ls":
                    this.TryTraverseFolders(input, data);
                    break;
                case "cmp":
                    this.TryCompareFiles(input, data);
                    break;
                case "cdRel":
                    this.TryChangePathRelatively(input, data);
                    break;
                case "cdAbs":
                    this.TryChangePathAbsolutely(input, data);
                    break;
                case "readDb":
                    this.TryReadDatabaseFromFile(input, data);
                    break;
                case "show":
                    this.TryShowWantedData(input, data);
                    break;
                case "filter":
                    this.TryFilterAndTake(input, data);
                    break;
                case "order":
                    this.TryOrderAndTake(input, data);
                    break;
                case "dropDb:":
                    this.TryDropDatabase(input, data);
                    break;
                case "help":
                    this.TryGetHelp(input, data);
                    break;
                default:
                    this.DisplayInvalidCommandMessage(input);
                    break;
            }
        }

        /// <summary>
        /// Try to open file in current path.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryOpenFile(string input, string[] data)
        {
            if (data.Length != 2)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }

            var fileName = data[1];
            Process.Start(SessionData.CurrentPath + "\\" + fileName);
        }

        /// <summary>
        /// Try to create directory in current folder.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryCreateDirectory(string input, string[] data)
        {
            if (data.Length != 2)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }

            var folderName = data[1];
            this.inputOutputManager.CreateDirectoryInCurrentFolder(folderName);
        }

        /// <summary>
        /// Try to traverse folder.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryTraverseFolders(string input, string[] data)
        {
            if (data.Length == 1)
            {
                this.inputOutputManager.TraverseDirectory(0);
            }
            else if (data.Length == 2)
            {
                var hasParsed = int.TryParse(data[1], out int depth);

                if (hasParsed)
                {
                    this.inputOutputManager.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
            }
        }

        /// <summary>
        /// Try to compate two files.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryCompareFiles(string input, string[] data)
        {
            if (data.Length == 3)
            {
                var firstPath = data[1];
                var secondPath = data[2];
                this.tester.CompareContent(firstPath, secondPath);
            }
            else
            {
                this.DisplayInvalidCommandMessage(input);
            }
        }

        /// <summary>
        /// Try to change path relatively.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryChangePathRelatively(string input, string[] data)
        {
            if (data.Length != 2)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }
            
            var relPath = data[1];
            this.inputOutputManager.ChangeCurrentDirectoryRelative(relPath);
        }

        /// <summary>
        /// Try to change path absolutely.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryChangePathAbsolutely(string input, string[] data)
        {
            if (data.Length != 2)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }
            
            var absolutePath = data[1];
            this.inputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }

        /// <summary>
        /// Try to read database from file.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryReadDatabaseFromFile(string input, string[] data)
        {
            if (data.Length != 2)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }
            
            var fileName = data[1];
            this.studentsRepository.LoadData(fileName);
        }

        /// <summary>
        /// Try show wanted data.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryShowWantedData(string input, string[] data)
        {
            if (data.Length == 2)
            {
                var courseName = data[1];
                this.studentsRepository.GetAllStudentsFromCourse(courseName);
            }
            else if (data.Length == 3)
            {
                var courseName = data[1];
                var userName = data[2];
                this.studentsRepository.GetStudentScoresFromCourse(courseName, userName);
            }
            else
            {
                this.DisplayInvalidCommandMessage(input);
            }
        }

        /// <summary>
        /// Try filter and take student data.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryFilterAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                var courseName = data[1];
                var filter = data[2];
                var takeCommand = data[3].ToLower();
                var takeQuantity = data[4].ToLower();
            
                this.TryParseParametersForFilterAndTake(takeCommand, takeQuantity, courseName, filter);
            }
            else
            {
                this.DisplayInvalidCommandMessage(input);
            }
        }

        private void TryParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    this.studentsRepository.FilterAndTake(courseName, filter);
                }
                else
                {
                    var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                    if (hasParsed)
                    {
                        this.studentsRepository.FilterAndTake(courseName, filter, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
            }
        }

        /// <summary>
        /// Try order and take student data.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryOrderAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                var orderCommand = data[0].ToLower();
                var courseName = data[1];
                var comparison = data[2];
                var takeQuantity = data[4].ToLower();

                this.TryParseParametersForOrderAndTake(orderCommand, takeQuantity, courseName, comparison);
            }
            else
            {
                this.DisplayInvalidCommandMessage(input);
            }
        }

        private void TryParseParametersForOrderAndTake(string orderCommand, string takeQuantity, string courseName, string comparison)
        {
            if (orderCommand == "order")
            {
                if (takeQuantity == "all")
                {
                    this.studentsRepository.OrderAndTake(courseName, comparison);
                }
                else
                {
                    var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                    if (hasParsed)
                    {
                        this.studentsRepository.OrderAndTake(courseName, comparison, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
            }
        }

        /// <summary>
        /// Try to drop the database.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryDropDatabase(string input, string[] data)
        {
            if (data.Length != 1)
            {
                this.DisplayInvalidCommandMessage(input);
                return;
            }

            this.studentsRepository.UnloadData();
            OutputWriter.WriteMessageOnNewLine("Database dropped");
        }

        /// <summary>
        /// Try to get help.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private void TryGetHelp(string input, string[] data)
        {
            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteMessageOnNewLine($"|{"make directory - mkdir: path ",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"traverse directory - ls: depth ",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"comparing files - cmp: path1 path2",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"change directory - changeDirREl:relative path",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"change directory - changeDir:absolute path",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"read students data base - readDb: path",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"filter {courseName} excelent/average/poor  take 2/5/all students - filterExcelent (the output is written on the console)",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"order increasing students - order {courseName} ascending/descending take 20/10/all (the output is written on the console)",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"download file - download: path of file (saved in current directory)",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"download file asinchronously - downloadAsynch: path of file (save in the current directory)",-98}|");
            OutputWriter.WriteMessageOnNewLine($"|{"get help – help",-98}|");
            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteEmptyLine();
        }

        /// <summary>
        /// Displays an invalid command message.
        /// </summary>
        /// <param studentByName="input">Input command.</param>
        private void DisplayInvalidCommandMessage(string input)
        {
            OutputWriter.DisplayException($"The command {input} is invalid");
        }
    }
}