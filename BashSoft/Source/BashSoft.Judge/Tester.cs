﻿namespace BashSoft.Judge
{
    using System;
    using System.IO;
    using Contracts;
    using Exceptions;
    using Output;
    using StaticData;

    public class Tester : IContentComparer
    {
        /// <summary>
        /// Compare two files and get mismatches.
        /// </summary>
        /// <param name="userOutputPath">User output path.</param>
        /// <param name="expectedOutputPath">Expected output path.</param>
        /// <exception cref="FileNotFoundException">
        /// If the file is with invalid path, or does not exist.</exception>
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            try
            {
                var mismatchesPath = this.GetMismatcPath(expectedOutputPath);
                var actualOutputLines = File.ReadAllLines(userOutputPath);
                var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                var mismatches =
                    this.GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out bool hasMismatch);

                this.PrintOutput(mismatches, hasMismatch, mismatchesPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch (FileNotFoundException)
            {
                throw new InvalidPathException();
            }
        }

        /// <summary>
        /// Get mismatches path and store the text file in this path.
        /// </summary>
        /// <param name="expectedOutputPath">Expected output path.</param>
        /// <returns>String with filan path.</returns>
        private string GetMismatcPath(string expectedOutputPath)
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
        /// <returns>Array with all mismathces.</returns>
        private string[] GetLinesWithPossibleMismatches(string[] actualOutputLines, string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            var output = string.Empty;
            var minOuputLines = actualOutputLines.Length;

            if (minOuputLines != expectedOutputLines.Length)
            {
                hasMismatch = true;
                minOuputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }

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
        /// <exception cref="DirectoryNotFoundException">
        /// If the file is with invalid path, or does not exist.</exception>
        private void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchesPath)
        {   
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                try
                {
                    File.WriteAllLines(mismatchesPath, mismatches);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new InvalidPathException();
                }

                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }
    }
}