﻿namespace BashSoft.Input.Commands
{
    using System.Diagnostics;
    using Exceptions;
    using Input.Contracts;
    using Judge.Contracts;
    using Repositories.Contracts;
    using StaticData;

    /// <summary>
    /// Open file in current path.
    /// </summary>
    public class OpenFileCommand : Command
    {
        /// <summary>
        /// Create a new instance of <see cref="OpenFileCommand"/>
        /// </summary>
        /// <param name="input">Input command.</param>
        /// <param name="data">Data.</param>
        /// <param name="judge">Tester.</param>
        /// <param name="repository">Student repository.</param>
        /// <param name="inputOutputManager">Input output manager.</param>
        public OpenFileCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
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
            Process.Start(SessionData.CurrentPath + "\\" + fileName);
        }
    }
}