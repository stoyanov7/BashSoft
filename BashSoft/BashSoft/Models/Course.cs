namespace BashSoft.Models
{
    using System.Collections.Generic;
    using Exceptions;
    using IO;

    public class Course
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExamTask = 100;

        public string name;
        public readonly Dictionary<string, Student> studentsByName;

        public Course(string name)
        {
            this.name = name;
            this.studentsByName = new Dictionary<string, Student>();
        }

        public void EnrollStudent(Student student)
        {
            if (this.studentsByName.ContainsKey(student.username))
            {
                OutputWriter.DisplayException(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse);
            }

            this.studentsByName[student.username] = student;
        }
    }
}