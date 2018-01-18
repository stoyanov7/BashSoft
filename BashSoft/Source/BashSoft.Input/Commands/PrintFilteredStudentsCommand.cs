namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class PrintFilteredStudentsCommand : Command
    {
        public PrintFilteredStudentsCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 5)
            {
                throw new InvalidCommandException(this.Input);
            }

            var courseName = this.Data[1];
            var filter = this.Data[2];
            var takeCommand = this.Data[3].ToLower();
            var takeQuantity = this.Data[4].ToLower();

            this.TryParseParametersForFilterAndTake(takeCommand, takeQuantity, courseName, filter);
        }

        private void TryParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        {
            if (takeCommand != "take")
            {
                throw new InvalidCommandException(takeCommand);
            }

            if (takeQuantity == "all")
            {
                this.Repository.FilterAndTake(courseName, filter);
            }
            else
            {
                var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                if (hasParsed)
                {
                    this.Repository.FilterAndTake(courseName, filter, studentsToTake);
                }
                else
                {
                    throw new InvalidCommandException(takeQuantity);
                }
            }
        }
    }
}