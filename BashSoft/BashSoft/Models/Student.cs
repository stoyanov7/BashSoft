namespace BashSoft.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Bytes2you.Validation;
    using Exceptions;
    using IO;
    using StaticData;

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
                Guard.WhenArgument(value, "username").IsNullOrEmpty().Throw();
                
                this.username = value;
            }
        }

        public IReadOnlyDictionary<string, Course> EnrolledCourses => this.enrolledCourses;

        public IReadOnlyDictionary<string, double> MarksByCourseName => this.marksByCourseName;

        public void EnrollInCourse(Course course)
        {
            Guard.WhenArgument(course, "course").IsNull().Throw();

            if (this.enrolledCourses.ContainsKey(course.Name))
            {
                throw new DuplicateEntryInStructureException(this.Username, course.Name);
            }

            this.enrolledCourses.Add(course.Name, course);
        }

        public void SetMarksInCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourses.ContainsKey(courseName))
            {
                throw new CourseNotFoundException();
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