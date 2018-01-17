namespace BashSoft.IO.Commands
{
    using System;
    using Exceptions;
    using Judge;
    using Repositories;

    public abstract class Command
    {
        private string input;
        private string[] data;
        private Tester judge;
        private StudentsRepository repository;
        private IoManager inputOutputManager;

        protected Command(string input, string[] data, Tester judge, StudentsRepository repository, IoManager inputOutputManager)
        {
            this.Input = input;
            this.Data = data;
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public string Input
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

        public string[] Data
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

        protected Tester Judge => this.judge;

        protected StudentsRepository Repository => this.repository;

        protected IoManager InputOutputManager => this.inputOutputManager;

        public abstract void Execute();
    }
}