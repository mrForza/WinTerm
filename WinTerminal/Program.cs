using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinTerminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            Terminal.DisplayGreeting();

            while (true)
            {
                Console.Write($"{Terminal.CheckBasePath()}/");
                CommandHandler.ReadCommands();
            }                           
        }
    }
}