namespace BashSoft.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Exceptions;
    using IO;
    using StaticData;

    public static class StudentsRepository
    {
        /// <summary>
        /// Boolean flag for whether the data structure is initialized.
        /// </summary>
        private static bool isDataInitialized = false;

        // Dictionary<courseName, Dictionary<userName, scoresOnTasks>>> 
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;

        /// <summary>
        /// Initialize and fill the data structure, if it is not initialized yet, reads the data.
        /// </summary>
        public static void InitializeData(string fileName = null)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInitialisedException);
            }
        }

        /// <summary>
        /// Get all the students scores on the tasks. 
        /// </summary>
        /// <param name="courseName">Course name.</param>
        /// <param name="username">Username.</param>
        public static void GetStudentScoresFromCourse(string courseName, string username)
        {
            if (IsQueryForStudentPossiblе(courseName, username))
            {
                OutputWriter.PrintStudent(
                    new KeyValuePair<string, List<int>>(username, studentsByCourse[courseName][username]));
            }
        }

        /// <summary>
        /// Get all students from a given course. 
        /// </summary>
        /// <param name="courseName">Course name.</param>
        public static void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}");
                foreach (var studetMarksEntry in studentsByCourse[courseName])
                {
                    OutputWriter.PrintStudent(studetMarksEntry);
                }
            }
        }

        /// <summary>
        /// Read the information for students.
        /// </summary>
        private static void ReadData()
        {
            var input = Console.ReadLine();

            while (!string.IsNullOrEmpty(input))
            {
                var tokens = input.Split(' ').ToArray();
                var course = tokens[0];
                var student = tokens[1];
                var mark = int.Parse(tokens[2]);

                // Add the course if the don't exist
                if (!studentsByCourse.ContainsKey(course))
                {
                    studentsByCourse.Add(course, new Dictionary<string, List<int>>());
                }

                // Add the student if the don't exist
                if (!studentsByCourse[course].ContainsKey(student))
                {
                    studentsByCourse[course].Add(student, new List<int>());
                }

                // Add the marks
                studentsByCourse[course][student].Add(mark);

                input = Console.ReadLine();
            }

            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private static void ReadData(string fileName)
        {
            var path = $"{SessionData.CurrentPath}\\{fileName}";

            if (File.Exists(path))
            {
                const string pattern = @"([A-Z][a-zA-Z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)";
                var regex = new Regex(pattern);
                var allInputLines = File.ReadAllLines(path);

                for (var line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = regex.Match(allInputLines[line]);
                        var courseName = currentMatch.Groups[1].Value;
                        var username = currentMatch.Groups[2].Value;
                        var hasParsedScore = int.TryParse(currentMatch.Groups[3].Value, out int studentScoreOnTask);

                        if (hasParsedScore && studentScoreOnTask >= 0 && studentScoreOnTask <= 100)
                        {
                            if (!studentsByCourse.ContainsKey(courseName))
                            {
                                studentsByCourse.Add(courseName, new Dictionary<string, List<int>>());
                            }

                            if (!studentsByCourse[courseName].ContainsKey(username))
                            {
                                studentsByCourse[courseName].Add(username, new List<int>());
                            }

                            studentsByCourse[courseName][username].Add(studentScoreOnTask);
                        }
                    }
                }
            }

            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        /// <summary>
        /// Check if query for course possible.
        /// </summary>
        /// <param name="courseName">Course name for checking.</param>
        /// <returns>
        /// True if if the data structure has been initialized and the course is contained,
        /// otherwise false.
        /// </returns>
        private static bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (studentsByCourse.ContainsKey(courseName))
                {
                    return true;
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            return false;
        }

        /// <summary>
        /// Check if query for student is possible.
        /// </summary>
        /// <param name="courseName">Course name for checking.</param>
        /// <param name="studentUsername">Student username for checking.</param>
        /// <seealso cref="IsQueryForCoursePossible"/>
        /// <returns>
        /// True if course is possible and student username is contained into database,
        /// otherwise false.
        /// </returns>
        private static bool IsQueryForStudentPossiblе(string courseName, string studentUsername)
        {
            if (IsQueryForCoursePossible(courseName) && studentsByCourse[courseName].ContainsKey(studentUsername))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }

            return false;
        }
    }
}