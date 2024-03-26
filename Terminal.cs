using System.IO;

namespace WinTerminal
{
    internal class Terminal
    {
        internal const string ROOTPATH = "C:";

        internal static string? Path { get; set; }


        internal static void DisplayGreeting()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            PrintFileText("InfoText/Greeting.txt");

            Console.ForegroundColor = ConsoleColor.White;
            PrintFileText("InfoText/AdditionalInfo.txt");
        }
 
        internal static void PrintFileText(string str)
        {
            using (StreamReader stream = new StreamReader($"../../../{str}"))
            {
                while (stream.EndOfStream != true)
                {
                    Console.WriteLine(stream.ReadLine());
                }
            }
        }

        internal static string CheckBasePath()
        {
            if (Path is not null)
            {
                return Path;
            }

            Path = ROOTPATH;
            return ROOTPATH;
        }

        internal static void DisplayError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        internal static void DisplayWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        internal static void DisplaySuccessMessage(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
