﻿namespace BashSoft.IO.Commands
{
    using Exceptions;
    using IO.Contracts;
    using Judge;
    using Repositories;

    public class ReadDatabaseCommand : Command
    {
        public ReadDatabaseCommand(string input, string[] data, Tester judge, StudentsRepository repository, IDirectoryManager inputOutputManager)
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