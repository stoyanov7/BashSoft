namespace BashSoft.Models.Contracts
{
    using System.Collections.Generic;

    public interface ICourse
    {
        string Name { get; }

        IReadOnlyDictionary<string, Student> StudentsByName { get; }

        void EnrollStudent(Student student);
    }
}