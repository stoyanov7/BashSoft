namespace BashSoft.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using IO;

    public static class RepositoryFilters
    {
        /// <summary>
        /// Filter and take students data.
        /// </summary>
        /// <param name="wantedData">Wanted data.</param>
        /// <param name="wantedFilter">Wanted filter.</param>
        /// <param name="studentsToTake">Students to take.</param>
        public static void FilterAndTake(Dictionary<string, List<int>> wantedData, string wantedFilter, int studentsToTake)
        {
            switch (wantedFilter)
            {
                case "excellent":
                    FilterAndTake(wantedData, x => x >= 5, studentsToTake);
                    break;
                case "average":
                    FilterAndTake(wantedData, x => x < 5 && x >= 3.5, studentsToTake);
                    break;
                case "poor":
                    FilterAndTake(wantedData, x => x < 3.5, studentsToTake);
                    break;
                default:
                    OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilter);
                    break;
            }
        }

        private static void FilterAndTake(Dictionary<string, List<int>> wantedData, Predicate<double> givenFilter, int studentsToTake)
        {
            var counterForPrinted = 0;

            foreach (var userName_Points in wantedData)
            {
                if (counterForPrinted == studentsToTake)
                {
                    break;
                }

                var averageMark = userName_Points.Value.Average();

                if (givenFilter(averageMark))
                {
                    OutputWriter.PrintStudent(userName_Points);
                    counterForPrinted++;
                }
            }
        }
    }
}