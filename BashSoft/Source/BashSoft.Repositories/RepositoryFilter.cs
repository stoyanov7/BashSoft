﻿namespace BashSoft.Repositories
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Exceptions;
    using Output;

    public class RepositoryFilter : IDataFilter
    {
        /// <summary>
        /// Filter and take students data.
        /// </summary>
        /// <param name="studentsWithMarks">Wanted data.</param>
        /// <param name="wantedFilter">Wanted filter.</param>
        /// <param name="studentsToTake">Students to take.</param>
        /// <exception cref="InvalidStudentFilterException"/>
        public void FilterAndTake(Dictionary<string, double> studentsWithMarks, string wantedFilter, int studentsToTake)
        {
            switch (wantedFilter)
            {
                case "excellent":
                    this.FilterAndTake(studentsWithMarks, x => x >= 5, studentsToTake);
                    break;
                case "average":
                    this.FilterAndTake(studentsWithMarks, x => x < 5 && x >= 3.5, studentsToTake);
                    break;
                case "poor":
                    this.FilterAndTake(studentsWithMarks, x => x < 3.5, studentsToTake);
                    break;
                default:
                    throw new InvalidStudentFilterException();
            }
        }

        private void FilterAndTake(Dictionary<string, double> studentsWithMarks, Predicate<double> givenFilter, int studentsToTake)
        {
            var counterForPrinted = 0;

            foreach (var studentMark in studentsWithMarks)
            {
                if (counterForPrinted == studentsToTake)
                {
                    break;
                }

                if (givenFilter(studentMark.Value))
                {
                    OutputWriter.PrintStudent(studentMark);
                    counterForPrinted++;
                }
            }
        }
    }
}