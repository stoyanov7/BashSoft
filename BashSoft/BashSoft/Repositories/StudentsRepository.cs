namespace BashSoft.Repositories
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Exceptions;
    using IO;
    using StaticData;

    public class StudentsRepository
    {
        private Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;
        private bool isDataInitialized = false;
        private RepositoryFilter filter;
        private RepositorySorter sorter;

        public StudentsRepository(RepositoryFilter filter, RepositorySorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
            this.studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }

                this.filter.FilterAndTake(studentsByCourse[courseName], givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }

                this.sorter.OrderAndTake(studentsByCourse[courseName], comparison, studentsToTake.Value);
            }
        }

        /// <summary>
        /// Initialize and fill the data structure, if it is not initialized yet, reads the data.
        /// </summary>
        public void LoadData(string fileName = null)
        {
            if (this.isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInitialisedException);
                return;
            }

            OutputWriter.WriteMessageOnNewLine("Reading data...");
            this.studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
            this.ReadData(fileName);
        }

        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            this.studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
            this.isDataInitialized = false;
        }

        /// <summary>
        /// Get all the students scores on the tasks. 
        /// </summary>
        /// <param name="courseName">Course name.</param>
        /// <param name="username">Username.</param>
        public void GetStudentScoresFromCourse(string courseName, string username)
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
        public void GetAllStudentsFromCourse(string courseName)
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

        private void ReadData(string fileName)
        {
            var path = $"{SessionData.CurrentPath}\\{fileName}";

            if (File.Exists(path))
            {
                const string Pattern = @"([A-Z][a-zA-Z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)";
                var regex = new Regex(Pattern);
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
        private bool IsQueryForCoursePossible(string courseName)
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
        private bool IsQueryForStudentPossiblе(string courseName, string studentUsername)
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