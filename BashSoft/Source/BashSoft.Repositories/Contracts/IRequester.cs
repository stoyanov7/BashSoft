namespace BashSoft.Repositories.Contracts
{
    public interface IRequester
    {
        void GetAllStudentsFromCourse(string courseName);
        void GetStudentScoresFromCourse(string courseName, string username);
    }
}