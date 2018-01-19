namespace BashSoft.Input
{
    using System;
    using Contracts;
    using Output;
    using StaticData;

    public class InputReader : IReader
    {
        private const string QuitCommand = "quit";
        private const string ExitCommand = "exit";

        private readonly ICommandInterpreter commandInterpreter;

        /// <summary>
        /// Create a new instance of <see cref="CommandInterpreter"/>
        /// </summary>
        /// <param name="commandInterpreter">Command interpreter.</param>
        public InputReader(ICommandInterpreter commandInterpreter)
        {
            this.commandInterpreter = commandInterpreter;
        }

        /// <summary>
        /// Reading commands from console.
        /// </summary>
        public void StartReadingCommands()
        {
            OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");
            var input = Console.ReadLine().Trim();
            
            while ((input != QuitCommand) && (input != ExitCommand))
            {
                this.commandInterpreter.InterpretCommand(input);
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");

                input = Console.ReadLine().Trim();
            }
        }
    }
}