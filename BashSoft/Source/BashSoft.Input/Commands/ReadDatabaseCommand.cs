﻿namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public class ReadDatabaseCommand : Command
    {
        public ReadDatabaseCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }

            var fileName = this.Data[1];
            this.Repository.LoadData(fileName);
        }
    }
}