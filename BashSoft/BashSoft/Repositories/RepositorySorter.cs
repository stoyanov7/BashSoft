namespace BashSoft.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using IO;

    public class RepositorySorter
    {
        /// <summary>
        /// Order and take students data.
        /// </summary>
        /// <param name="wantedData">Wanted data.</param>
        /// <param name="comparison">Comparasion.</param>
        /// <param name="studentToTake">Students to take.</param>
        public void OrderAndTake(Dictionary<string, List<int>> wantedData, string comparison, int studentToTake)
        {
            comparison = comparison.ToLower();

            switch (comparison)
            {
                case "ascending":
                    PrintStudents(wantedData
                        .OrderBy(x => x.Value.Sum())
                        .Take(studentToTake)
                        .ToDictionary(pair => pair.Key, pair => pair.Value));
                    break;
                case "descending":
                    PrintStudents(wantedData
                        .OrderByDescending(x => x.Value.Sum())
                        .Take(studentToTake)
                        .ToDictionary(pair => pair.Key, pair => pair.Value));
                    break;
                default:
                    OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQuery);
                    break;
            }
        }

        private void PrintStudents(Dictionary<string, List<int>> studentsSorted)
        {
            foreach (var kvp in studentsSorted)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }
    }
}