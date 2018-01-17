namespace BashSoft.IO.Commands
{
    using Exceptions;
    using IO.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class ShowCourseCommand : Command
    {
        public ShowCourseCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var courseName = this.Data[1];
                this.Repository.GetAllStudentsFromCourse(courseName);
            }
            else if (this.Data.Length == 3)
            {
                var courseName = this.Data[1];
                var userName = this.Data[2];
                this.Repository.GetStudentScoresFromCourse(courseName, userName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}