namespace BashSoft.IO.Commands
{
    using Exceptions;
    using IO.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class DropDatabaseCommand : Command
    {
        public DropDatabaseCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 1)
            {
                throw new InvalidCommandException(this.Input);
            }

            this.Repository.UnloadData();
            OutputWriter.WriteMessageOnNewLine("Database dropped");
        }
    }
}