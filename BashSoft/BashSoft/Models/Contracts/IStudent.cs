namespace BashSoft.Models.Contracts
{
    using System.Collections.Generic;

    public interface IStudent
    {
        IReadOnlyDictionary<string, Course> EnrolledCourses { get; }

        IReadOnlyDictionary<string, double> MarksByCourseName { get; }

        string Username { get; }

        void EnrollInCourse(Course course);
        void SetMarksInCourse(string courseName, params int[] scores);
    }
}