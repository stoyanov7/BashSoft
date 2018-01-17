namespace BashSoft.IO.Commands
{
    using Exceptions;
    using IO.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class ChangeAbsolutePathCommand : Command
    {
        public ChangeAbsolutePathCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }

            var absolutePath = this.Data[1];
            this.InputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }
    }
}