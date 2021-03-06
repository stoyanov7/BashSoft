﻿namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class CompareFilesCommand : Command
    {
        public CompareFilesCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 3)
            {
                var firstPath = this.Data[1];
                var secondPath = this.Data[2];
                this.Judge.CompareContent(firstPath, secondPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}