namespace BashSoft.StaticData
{
    public static class ExceptionMessages
    {
        public const string DataNotInitializedExceptionMessage =
            "The data structure must be initialised first in order to make any operations with it.";

        public const string InexistingCourseInDataBase =
            "The course you are trying to get does not exist in the data base!";

        public const string InexistingStudentInDataBase =
            "The user studentByName for the student you are trying to get does not exist!";

        public const string UnauthorizedAccessExceptionMessage =
            "The folder/file you are trying to get access needs a higher level of rights than you currently have.";

        public const string ComparisonOfFilesWithDifferentSizes = "Files not of equal size, certain mismatch.";

        public const string InvalidTakeQuantityParameter =
            "The take command expected does not match the format wanted!";

        public const string InvalidNumberOfScores = 
            "The number of scores for the given course is greater than the possible.";

        public const string InvalidScores = 
            "The number for the score you've entered is not in the range of 0 - 100";
    }
}