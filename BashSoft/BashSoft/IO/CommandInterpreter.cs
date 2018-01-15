namespace BashSoft.IO
{
    using System.Diagnostics;
    using System.Linq;
    using Exceptions;
    using Judge;
    using Repositories;
    using StaticData;

    public static class CommandInterpreter
    {
        public static void InterpretCommand(string input)
        {
            var data = input
                .Split(' ')
                .ToArray();

            var command = data[0];

            switch (command)
            {
                case "open":
                    TryOpenFile(input, data);
                    break;
                case "mkdir":
                    TryCreateDirectory(input, data);
                    break;
                case "ls":
                    TryTraverseFolders(input, data);
                    break;
                case "cmp":
                    TryCompareFiles(input, data);
                    break;
                case "cdRel":
                    TryChangePathRelatively(input, data);
                    break;
                case "cdAbs":
                    TryChangePathAbsolutely(input, data);
                    break;
                case "readDb":
                    TryReadDatabaseFromFile(input, data);
                    break;
                case "show":
                    TryShowWantedData(input, data);
                    break;
                case "filter":
                    TryFilterAndTake(input, data);
                    break;
                case "order":
                    TryOrderAndTake(input, data);
                    break;
                case "help":
                    TryGetHelp(input, data);
                    break;
                default:
                    DisplayInvalidCommandMessage(input);
                    break;
            }
        }

        /// <summary>
        /// Try to open file in current path.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryOpenFile(string input, string[] data)
        {
            if (data.Length != 2)
            {
                DisplayInvalidCommandMessage(input);
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
        private static void TryCreateDirectory(string input, string[] data)
        {
            if (data.Length != 2)
            {
                DisplayInvalidCommandMessage(input);
                return;
            }

            var folderName = data[1];
            IoManager.CreateDirectoryInCurrentFolder(folderName);
        }

        /// <summary>
        /// Try to traverse folder.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryTraverseFolders(string input, string[] data)
        {
            if (data.Length == 1)
            {
                IoManager.TraverseDirectory(0);
            }
            else if (data.Length == 2)
            {
                var hasParsed = int.TryParse(data[1], out int depth);

                if (hasParsed)
                {
                    IoManager.TraverseDirectory(depth);
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
        private static void TryCompareFiles(string input, string[] data)
        {
            if (data.Length == 3)
            {
                var firstPath = data[1];
                var secondPath = data[2];
                Tester.CompareContent(firstPath, secondPath);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        /// <summary>
        /// Try to change path relatively.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryChangePathRelatively(string input, string[] data)
        {
            if (data.Length != 2)
            {
                DisplayInvalidCommandMessage(input);
                return;
            }
            
            var relPath = data[1];
            IoManager.ChangeCurrentDirectoryRelative(relPath);
        }

        /// <summary>
        /// Try to change path absolutely.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryChangePathAbsolutely(string input, string[] data)
        {
            if (data.Length != 2)
            {
                DisplayInvalidCommandMessage(input);
                return;
            }
            
            var absolutePath = data[1];
            IoManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }

        /// <summary>
        /// Try to read database from file.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryReadDatabaseFromFile(string input, string[] data)
        {
            if (data.Length != 2)
            {
                DisplayInvalidCommandMessage(input);
                return;
            }
            
            var fileName = data[1];
            StudentsRepository.InitializeData(fileName);
        }

        /// <summary>
        /// Try show wanted data.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryShowWantedData(string input, string[] data)
        {
            if (data.Length == 2)
            {
                var courseName = data[1];
                StudentsRepository.GetAllStudentsFromCourse(courseName);
            }
            else if (data.Length == 3)
            {
                var courseName = data[1];
                var userName = data[2];
                StudentsRepository.GetStudentScoresFromCourse(courseName, userName);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        /// <summary>
        /// Try filter and take student data.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryFilterAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                var courseName = data[1];
                var filter = data[2];
                var takeCommand = data[3].ToLower();
                var takeQuantity = data[4].ToLower();
            
                TryParseParametersForFilterAndTake(takeCommand, takeQuantity, courseName, filter);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    StudentsRepository.FilterAndTake(courseName, filter);
                }
                else
                {
                    var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                    if (hasParsed)
                    {
                        StudentsRepository.FilterAndTake(courseName, filter, studentsToTake);
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
        private static void TryOrderAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                var orderCommand = data[0].ToLower();
                var courseName = data[1];
                var comparison = data[2];
                var takeQuantity = data[4].ToLower();

                TryParseParametersForOrderAndTake(orderCommand, takeQuantity, courseName, comparison);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryParseParametersForOrderAndTake(string orderCommand, string takeQuantity, string courseName, string comparison)
        {
            if (orderCommand == "order")
            {
                if (takeQuantity == "all")
                {
                    StudentsRepository.OrderAndTake(courseName, comparison);
                }
                else
                {
                    var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                    if (hasParsed)
                    {
                        StudentsRepository.OrderAndTake(courseName, comparison, studentsToTake);
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
        /// Try to get help.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="data">Data.</param>
        private static void TryGetHelp(string input, string[] data)
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
        /// <param name="input">Input command.</param>
        private static void DisplayInvalidCommandMessage(string input)
        {
            OutputWriter.DisplayException($"The command {input} is invalid");
        }
    }
}