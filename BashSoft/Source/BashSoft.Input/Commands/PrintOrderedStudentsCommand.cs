namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class PrintOrderedStudentsCommand : Command
    {
        public PrintOrderedStudentsCommand(string input, string[] data, IContentComparer judge, IDatabase repository,IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 5)
            {
                throw new InvalidCommandException(this.Input);
            }

            var orderCommand = this.Data[0].ToLower();
            var courseName = this.Data[1];
            var comparison = this.Data[2];
            var takeQuantity = this.Data[4].ToLower();

            this.TryParseParametersForOrderAndTake(orderCommand, takeQuantity, courseName, comparison);
        }

        private void TryParseParametersForOrderAndTake(string orderCommand, string takeQuantity, string courseName, string comparison)
        {
            if (orderCommand != "order")
            {
                throw new InvalidCommandException(orderCommand);
            }

            if (takeQuantity == "all")
            {
                this.Repository.OrderAndTake(courseName, comparison);
            }
            else
            {
                var hasParsed = int.TryParse(takeQuantity, out int studentsToTake);

                if (hasParsed)
                {
                    this.Repository.OrderAndTake(courseName, comparison, studentsToTake);
                }
                else
                {
                    throw new InvalidCommandException(takeQuantity);
                }
            }
        }
    }
}