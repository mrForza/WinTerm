using System.IO;

namespace WinTerminal.Commands
{
    internal class MainCommand
    {
        internal static void Help()
        {
            Terminal.PrintFileText("CommandsText/CommandsDescription.txt");
        }

        internal static void Info()
        {
            Terminal.DisplayGreeting();
        }

        internal static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
