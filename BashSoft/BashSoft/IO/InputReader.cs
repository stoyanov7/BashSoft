namespace BashSoft.IO
{
    using System;
    using StaticData;

    public class InputReader
    {
        private const string QuitCommand = "quit";
        private const string ExitCommand = "exit";

        private CommandInterpreter commandInterpreter;

        public InputReader(CommandInterpreter commandInterpreter)
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