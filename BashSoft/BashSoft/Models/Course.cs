namespace BashSoft.Models
{
    using System.Collections.Generic;
    using Bytes2you.Validation;
    using Contracts;
    using Exceptions;

    public class Course : ICourse
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExamTask = 100;

        private string name;
        private readonly Dictionary<string, IStudent> studentsByName;

        public Course(string name)
        {
            this.Name = name;
            this.studentsByName = new Dictionary<string, IStudent>();
        }

        public string Name
        {
            get => this.name;

            private set
            {
                Guard.WhenArgument(value, "Name").IsNullOrEmpty().Throw();
                
                this.name = value;
            }
        }

        public IReadOnlyDictionary<string, IStudent> StudentsByName => this.studentsByName;

        public void EnrollStudent(IStudent student)
        {
            Guard.WhenArgument(student, "student").IsNull().Throw();

            if (this.studentsByName.ContainsKey(student.Username))
            {
                throw new DuplicateEntryInStructureException(student.Username, this.Name);
            }

            this.studentsByName[student.Username] = student;
        }
    }
}