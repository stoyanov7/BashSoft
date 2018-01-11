namespace BashSoft.IO
{
    using System;
    using StaticData;

    public static class InputReader
    {
        private const string QuitCommand = "quit";
        private const string ExitCommand = "exit";

        public static void StartReadingCommands()
        {
            OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");
            var input = Console.ReadLine().Trim();
            
            while ((input != QuitCommand) && (input != ExitCommand))
            {
                CommandInterpreter.InterpretCommand(input);
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");

                input = Console.ReadLine().Trim();
            }
        }
    }
}