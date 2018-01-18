namespace BashSoft.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Contracts;
    using Exceptions;
    using Models;
    using Models.Contracts;
    using Output;
    using StaticData;

    public class StudentsRepository : IDatabase
    {
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;
        private bool isDataInitialized;
        private readonly IDataFilter filter;
        private readonly IDataSorter sorter;

        /// <summary>
        /// Create new instance of <see cref="RepositoryFilter"/> and <see cref="RepositorySorter"/>
        /// </summary>
        /// <param name="filter">Repository filter.</param>
        /// <param name="sorter">Repository sorter.</param>
        public StudentsRepository(IDataFilter filter, IDataSorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
            this.students = new Dictionary<string, IStudent>();
            this.courses = new Dictionary<string, ICourse>();
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                var marks =
                    this.courses[courseName]
                    .StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

                this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                var marks =
                    this.courses[courseName]
                    .StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }

        /// <summary>
        /// Initialize and fill the data structure, if it is not initialized yet, reads the data.
        /// </summary>
        /// <exception cref="DataAlreadyInitialisedException"/>
        public void LoadData(string fileName = null)
        {
            if (this.isDataInitialized)
            {
                throw new DataAlreadyInitialisedException();
            }

            this.students = new Dictionary<string, IStudent>();
            this.courses = new Dictionary<string, ICourse>();

            OutputWriter.WriteMessageOnNewLine("Reading data...");
            this.ReadData(fileName);
        }

        /// <summary>
        /// Unload data structure.
        /// </summary>
        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;
        }

        /// <summary>
        /// Get students scores from given course.
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="username"></param>
        public void GetStudentScoresFromCourse(string courseName, string username)
        {
            if (this.IsQueryForStudentPossiblе(courseName, username)
                && this.courses[courseName].StudentsByName.ContainsKey(username))
            {
                OutputWriter.PrintStudent(
                    new KeyValuePair<string, double>(
                        username,this.courses[courseName].StudentsByName[username].MarksByCourseName[courseName]));
            }
        }

        /// <summary>
        /// Get all students from a given course. 
        /// </summary>
        /// <param name="courseName">Course studentByName.</param>
        public void GetAllStudentsFromCourse(string courseName)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");

                foreach (var studentMarksEntry in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentScoresFromCourse(courseName, studentMarksEntry.Key);
                }
            }
        }

        /// <summary>
        /// Read the data.
        /// </summary>
        /// <param name="fileName">File name for reading the data.</param>
        /// <exception cref="FormatException"/>
        private void ReadData(string fileName)
        {
            var path = $"{SessionData.CurrentPath}\\{fileName}";

            if (File.Exists(path))
            {
                const string Pattern = @"([A-Z][a-zA-Z#\++]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
                var regex = new Regex(Pattern);
                var allInputLines = File.ReadAllLines(path);

                for (var line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = regex.Match(allInputLines[line]);
                        var courseName = currentMatch.Groups[1].Value;
                        var username = currentMatch.Groups[2].Value;
                        var scoresStr = currentMatch.Groups[3].Value;

                        try
                        {
                            var scores = scoresStr
                                                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(int.Parse)
                                                .ToArray();

                            if (scores.Any(n => n > 100 || n < 0))
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScores);
                            }

                            if (scores.Length > Course.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                                continue;
                            }

                            if (!this.students.ContainsKey(username))
                            {
                                this.students.Add(username, new Student(username));
                            }

                            if (!this.courses.ContainsKey(courseName))
                            {
                                this.courses.Add(courseName, new Course(courseName));
                            }

                            var course = this.courses[courseName];
                            var student = this.students[username];

                            student.EnrollInCourse(course);
                            student.SetMarksInCourse(courseName, scores);
                        }
                        catch (FormatException fex)
                        {
                            OutputWriter.DisplayException($"{fex.Message} at line : {line}");
                        }
                    }
                }
            }

            this.isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        /// <summary>
        /// Check if query for course possible.
        /// </summary>
        /// <param name="courseName">Course studentByName for checking.</param>
        /// <returns>
        /// True if if the data structure has been initialized and the course is contained,
        /// otherwise false.
        /// </returns>
        private bool IsQueryForCoursePossible(string courseName)
        {
            if (this.isDataInitialized)
            {
                if (this.courses.ContainsKey(courseName))
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
        /// <param name="courseName">Course studentByName for checking.</param>
        /// <param name="studentUsername">Student username for checking.</param>
        /// <seealso cref="IsQueryForCoursePossible"/>
        /// <returns>
        /// True if course is possible and student username is contained into database,
        /// otherwise false.
        /// </returns>
        private bool IsQueryForStudentPossiblе(string courseName, string studentUsername)
        {
            if (this.IsQueryForCoursePossible(courseName)
                && this.courses[courseName].StudentsByName.ContainsKey(studentUsername))
            {
                return true;
            }

            OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);

            return false;
        }
    }
}