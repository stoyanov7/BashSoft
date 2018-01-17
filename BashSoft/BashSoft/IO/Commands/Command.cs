namespace BashSoft.IO.Commands
{
    using System;
    using Contracts;
    using Exceptions;
    using IO.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    public abstract class Command : IExecutable
    {
        private string input;
        private string[] data;
        private readonly IContentComparer judge;
        private readonly IDatabase repository;
        private readonly IDirectoryManager inputOutputManager;

        protected Command(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.Input = input;
            this.Data = data;
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        protected string Input
        {
            get => this.input;

            private set
            {
                //TODO: Make and use Validator class
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }

                this.input = value;
            }
        }

        protected string[] Data
        {
            get => this.data;

            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new NullReferenceException();
                }

                this.data = value;
            }
        }

        protected IContentComparer Judge => this.judge;

        protected IDatabase Repository => this.repository;

        protected IDirectoryManager InputOutputManager => this.inputOutputManager;

        public abstract void Execute();
    }
}