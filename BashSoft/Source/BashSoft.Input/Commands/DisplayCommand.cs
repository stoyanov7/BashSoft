namespace BashSoft.Input.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Models.Contracts;
    using Output;
    using Repositories.Contracts;

    public class DisplayCommand : Command
    {
        public DisplayCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 3)
            {
                throw new InvalidCommandException(this.Input);
            }

            var entityToDisplay = this.Data[1];
            var sortType = this.Data[2];

            if (entityToDisplay.Equals("students", StringComparison.OrdinalIgnoreCase))
            {
                var studentComparator = this.CreateStudentComparator(sortType);
                var students = this.Repository.GetAllStudentsSorted(studentComparator);
                OutputWriter.WriteMessageOnNewLine(students.JoinWith(Environment.NewLine));
            }
            else if (entityToDisplay.Equals("courses", StringComparison.OrdinalIgnoreCase))
            {
                var courseComparator = this.CreateCourseComparator(sortType);
                var courses = this.Repository.GetAllCoursesSorted(courseComparator);
                OutputWriter.WriteMessageOnNewLine(courses.JoinWith(Environment.NewLine));
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private IComparer<IStudent> CreateStudentComparator(string sortType)
        {
            if (sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<IStudent>.Create((s1, s2) => s1.CompareTo(s2));
            }

            if (sortType.Equals("descending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<IStudent>.Create((s1, s2) => s2.CompareTo(s1));
            }

            throw new InvalidCommandException(this.Input);
        }

        private IComparer<ICourse> CreateCourseComparator(string sortType)
        {
            if (sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<ICourse>.Create((c1, c2) => c1.CompareTo(c2));
            }

            if (sortType.Equals("descending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<ICourse>.Create((c1, c2) => c2.CompareTo(c1));
            }

            throw new InvalidCommandException(this.Input);
        }
    }
}