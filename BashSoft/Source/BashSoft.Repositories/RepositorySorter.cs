namespace BashSoft.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Exceptions;
    using Output;

    public class RepositorySorter : IDataSorter
    {
        /// <summary>
        /// Order and take students data.
        /// </summary>
        /// <param name="studentsMarks">Wanted data.</param>
        /// <param name="comparison">Comparasion.</param>
        /// <param name="studentToTake">Students to take.</param>
        /// <exception cref="InvalidComparisonQueryException"/>
        public void OrderAndTake(Dictionary<string, double> studentsMarks, string comparison, int studentToTake)
        {
            comparison = comparison.ToLower();

            switch (comparison)
            {
                case "ascending":
                    this.PrintStudents(studentsMarks
                        .OrderBy(x => x.Value)
                        .Take(studentToTake)
                        .ToDictionary(pair => pair.Key, pair => pair.Value));
                    break;
                case "descending":
                    this.PrintStudents(studentsMarks
                        .OrderByDescending(x => x.Value)
                        .Take(studentToTake)
                        .ToDictionary(pair => pair.Key, pair => pair.Value));
                    break;
                default:
                    throw new InvalidComparisonQueryException();
            }
        }

        /// <summary>
        /// Print students.
        /// </summary>
        /// <param name="studentsSorted">Students for printing.</param>
        private void PrintStudents(Dictionary<string, double> studentsSorted)
        {
            foreach (var kvp in studentsSorted)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }
    }
}