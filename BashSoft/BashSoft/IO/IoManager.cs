namespace BashSoft.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Exceptions;
    using StaticData;

    public class IoManager
    {
        /// <summary>
        /// Traverse folders in order.
        /// </summary>
        /// <param name="depth">Directory depth.</param>
        /// <exception cref="UnauthorizedAccessException">
        /// If The folder/file you are trying to get access needs a higher level of rights.</exception>
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
            var path = SessionData.CurrentPath + "\\" + folderName;

            if (Directory.Exists(path))
            {
                OutputWriter.WriteMessageOnNewLine($"File {folderName} exist!");
                return;
            }

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                OutputWriter.DisplayException(ExceptionMessages.ForbiddenSymbolsContainedInName);   
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
                    OutputWriter.DisplayException(ExceptionMessages.UnableToGoHigherInPartitionHierarchy);
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
        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                return;
            }

            SessionData.CurrentPath = absolutePath;
        }
    }
}