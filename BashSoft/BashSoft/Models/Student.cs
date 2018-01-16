namespace BashSoft.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using IO;

    public class Student
    {
        private string username;
        private readonly Dictionary<string, Course> enrolledCourses;
        private readonly Dictionary<string, double> marksByCourseName;

        public Student(string username)
        {
            this.Username = username;
            this.enrolledCourses = new Dictionary<string, Course>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public string Username
        {
            get => this.username;

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(this.username), ExceptionMessages.NullOrEmptyValue);
                }

                this.username = value;
            }
        }

        public IReadOnlyDictionary<string, Course> EnrolledCourses => this.enrolledCourses;

        public IReadOnlyDictionary<string, double> MarksByCourseName => this.marksByCourseName;

        public void EnrollInCourse(Course course)
        {
            if (this.enrolledCourses.ContainsKey(course.Name))
            {
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, this.username, course.Name));
                return;
            }

            this.enrolledCourses.Add(course.Name, course);
        }

        public void SetMarksInCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourses.ContainsKey(courseName))
            {
                OutputWriter.DisplayException(ExceptionMessages.NotEnrolledInCourse);
                return;
            }

            if (scores.Length > Course.NumberOfTasksOnExam)
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                return;
            }

            this.marksByCourseName[courseName] = this.CalculateMark(scores);
        }

        private double CalculateMark(int[] scores)
        {
            var percentageOfSolvedExam = scores.Sum()
                / (double)(Course.NumberOfTasksOnExam * Course.MaxScoreOnExamTask);
            var mark = percentageOfSolvedExam * 4 + 2;

            return mark;
        }
    }
}