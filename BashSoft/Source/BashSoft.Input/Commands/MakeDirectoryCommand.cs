namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    /// <summary>
    /// Try to create directory in current folder.
    /// </summary>
    public class MakeDirectoryCommand : Command
    {
        /// <summary>
        /// Create a new instance of <see cref="MakeDirectoryCommand"/>
        /// </summary>
        /// <param name="input">Input command.</param>
        /// <param name="data">Data.</param>
        /// <param name="judge">Tester.</param>
        /// <param name="repository">Student repository.</param>
        /// <param name="inputOutputManager">Input output manager.</param>
        public MakeDirectoryCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }

            var folderName = this.Data[1];
            this.InputOutputManager.CreateDirectoryInCurrentFolder(folderName);
        }
    }
}