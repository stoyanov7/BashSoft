namespace BashSoft.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using IO;

    public class Student
    {
        public string username;
        public Dictionary<string, Course> enrolledCourses;
        public Dictionary<string, double> marksByCourseName;

        public Student(string username)
        {
            this.username = username;
            this.enrolledCourses = new Dictionary<string, Course>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public void EnrollCourse(Course course)
        {
            if (this.enrolledCourses.ContainsKey(course.name))
            {
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, this.username, course.name));
                return;
            }

            this.enrolledCourses.Add(course.name, course);
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

            this.marksByCourseName[courseName] = CalculateMark(scores);
        }

        private double CalculateMark(int[] scores)
        {
            var percentageOfSolvedExam = scores.Sum() / (double)(Course.NumberOfTasksOnExam * Course.MaxScoreOnExamTask);
            var mark = percentageOfSolvedExam * 4 + 2;

            return mark;
        }
    }
}