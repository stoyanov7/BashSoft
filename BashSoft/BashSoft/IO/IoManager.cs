namespace BashSoft
{
    using System.Collections.Generic;
    using System.IO;

    public static class IoManager
    {
        /// <summary>
        /// Traverse folders in order.
        /// </summary>
        /// <param name="path">Directory path.</param>
        public static void TraverseDirectory(string path)
        {
            OutputWriter.WriteEmptyLine();
            var initialIdentation = path.Split('\\').Length;
            var subFolders = new Queue<string>();
            subFolders.Enqueue(path);

            while (subFolders.Count != 0)
            {
                // Deque the folder at the start ot the queue
                var currentPath = subFolders.Dequeue();
                var identation = currentPath.Split('\\').Length - initialIdentation;

                // Print the folder path
                OutputWriter.WriteMessageOnNewLine($"{new string('-', identation)} {currentPath}");

                foreach (var directoryPath in Directory.GetDirectories(currentPath))
                {
                    // Add all it's subfolders to the end of the queue 
                    subFolders.Enqueue(directoryPath);
                }
            }
        }
    }
}