namespace BashSoft.Input.Commands
{
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;

    /// <summary>
    /// Try to traverse folder.
    /// </summary>
    public class TraverseFoldersCommand : Command
    {
        /// <summary>
        /// Create a new instance of <see cref="TraverseFoldersCommand"/>
        /// </summary>
        /// <param name="input">Input command.</param>
        /// <param name="data">Data.</param>
        /// <param name="judge">Tester.</param>
        /// <param name="repository">Student repository.</param>
        /// <param name="inputOutputManager">Input output manager.</param>
        public TraverseFoldersCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 1)
            {
                this.InputOutputManager.TraverseDirectory(0);
            }
            else if (this.Data.Length == 2)
            {
                var hasParsed = int.TryParse(this.Data[1], out int depth);

                if (hasParsed)
                {
                    this.InputOutputManager.TraverseDirectory(depth);
                }
                else
                {
                    throw new InvalidNumberException(this.Data[1]);
                }
            }
        }
    }
}