namespace BashSoft.Judge
{
    using System;
    using System.IO;
    using IO;

    public class Tester
    {
        /// <summary>
        /// Compare two files and get mismatches.
        /// </summary>
        /// <param name="userOutputPath">User output path.</param>
        /// <param name="expectedOutputPath">Expected output path.</param>
        public static void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");
            var mismatchesPath = GetMismatcPath(expectedOutputPath);
            var actualOutputLines = File.ReadAllLines(userOutputPath);
            var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

            var mismatches = 
                GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out bool hasMismatch);

            PrintOutput(mismatches, hasMismatch, mismatchesPath);
            OutputWriter.WriteMessageOnNewLine("Files read!");
        }

        /// <summary>
        /// Get mismatches path and store the text file in this path.
        /// </summary>
        /// <param name="expectedOutputPath">Expected output path.</param>
        /// <returns>String with filan path.</returns>
        private static string GetMismatcPath(string expectedOutputPath)
        {
            var indexOfLastSlash = expectedOutputPath.LastIndexOf('\\');
            var directoryPath = expectedOutputPath.Substring(0, indexOfLastSlash);
            var finalPath = directoryPath + @"Mismatches.txt";

            return finalPath;
        }

        /// <summary>
        /// Compare files and get lines with mismatches.
        /// </summary>
        /// <param name="actualOutputLines">Actual output lines.</param>
        /// <param name="expectedOutputLines">Expected output lines.</param>
        /// <param name="hasMismatch">Has any mismatches.</param>
        /// <returns></returns>
        private static string[] GetLinesWithPossibleMismatches(string[] actualOutputLines,
            string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            var output = string.Empty;
            var minOuputLines = actualOutputLines.Length;

            var mismatches = new string[minOuputLines];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (var i = 0; i < minOuputLines; i++)
            {
                var actualLine = actualOutputLines[i];
                var expectedLine = expectedOutputLines[i];

                if (!actualLine.Equals(expectedLine))
                {
                    // Create mismatching line for "Mismatches.txt
                    output = string.Format($"Mismatch at line {i} -- expected: \"{expectedLine}\", actual: \"{actualLine}\"");
                    output += Environment.NewLine;
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }

                mismatches[i] = output;
            }

            return mismatches;
        }

        /// <summary>
        /// Print the output on file.
        /// </summary>
        /// <param name="mismatches">Array with mismatches.</param>
        /// <param name="hasMismatch">Has any mismatches.</param>
        /// <param name="mismatchesPath">Mismatches path.</param>
        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchesPath)
        {   
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchesPath, mismatches);
                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }
    }
}