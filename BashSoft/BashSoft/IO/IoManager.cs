namespace BashSoft.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Bytes2you.Validation;
    using Contracts;
    using Exceptions;
    using StaticData;

    public class IoManager : IDirectoryManager
    {
        /// <summary>
        /// Traverse folders in order.
        /// </summary>
        /// <param name="depth">Directory depth.</param>
        /// <exception cref="UnauthorizedAccessException">
        /// If the folder/file you are trying to get access needs a higher level of rights.</exception>
        public void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            var initialIdentation = SessionData.CurrentPath.Split('\\').Length;
            var subFolders = new Queue<string>();
            subFolders.Enqueue(SessionData.CurrentPath);

            while (subFolders.Count != 0)
            {
                // Deque the folder at the start ot the queue
                var currentPath = subFolders.Dequeue();
                var identation = currentPath.Split('\\').Length - initialIdentation;

                if (depth - identation < 0)
                {
                    break;
                }

                foreach (var directoryPath in Directory.GetDirectories(currentPath))
                {
                    subFolders.Enqueue(directoryPath);
                }

                // Print the folder path
                OutputWriter.WriteMessageOnNewLine($"{new string('-', identation)} {currentPath}");

                try
                {
                    foreach (var file in Directory.GetFiles(SessionData.CurrentPath))
                    {
                        var indexOfLastSlash = file.LastIndexOf("\\");
                        var fileName = file.Substring(indexOfLastSlash);
                        OutputWriter.WriteMessageOnNewLine(new string('-', indexOfLastSlash) + fileName);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessExceptionMessage);
                }
            }
        }

        /// <summary>
        /// Create directory in current path.
        /// </summary>
        /// <param name="folderName">Folder name.</param>
        /// <exception cref="ArgumentException">
        /// If the folder name contains symbols that are not allowed to be used.</exception>
        public void CreateDirectoryInCurrentFolder(string folderName)
        {
            Guard.WhenArgument(folderName, "folderName").IsNullOrEmpty().Throw();

            var path = SessionData.CurrentPath + "\\" + folderName;

            if (Directory.Exists(path))
            {
                // TODO: Add custom exception - FileExistException
                OutputWriter.WriteMessageOnNewLine($"File {folderName} exist!");
                return;
            }

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
            }

            OutputWriter.WriteMessageOnNewLine("Directory created!");
        }

        /// <summary>
        /// Moves forwards and backwards in the path.
        /// </summary>
        /// <param name="relativePath">Relative path.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the path is cannot go in higher level.</exception>
        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            Guard.WhenArgument(relativePath, "relativePath").IsNullOrEmpty().Throw();

            if (relativePath == "..")
            {
                try
                {
                    var currentPath = SessionData.CurrentPath;
                    var indexOfLastSlash = currentPath.LastIndexOf("\\");
                    var newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.CurrentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new InvalidPathException();
                }
            }
            else
            {
                var currenPath = SessionData.CurrentPath;
                currenPath += "\\" + relativePath;
                this.ChangeCurrentDirectoryAbsolute(currenPath);
            }
        }

        /// <summary>
        /// Get absolute path and goes directly to the path.
        /// </summary>
        /// <param name="absolutePath">Absolute path.</param>
        /// <exception cref="InvalidPathException">
        /// If the file/folder is with invalid path or does not exist.</exception>
        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            Guard.WhenArgument(absolutePath, "absolutePath").IsNullOrEmpty().Throw();

            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();
            }

            SessionData.CurrentPath = absolutePath;
        }
    }
}