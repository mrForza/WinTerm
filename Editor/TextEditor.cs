using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTerminal.Editor
{
    internal class TextEditor
    {
        private static bool _isUsed = false;

        internal static char[][] DisplayFileContent(string name)
        {
            Console.Clear();

            char[][] buffer = new char[100][];

            using (StreamReader stream = new StreamReader(@$"{Terminal.Path}/{name}"))
            {
                int index = 0;

                while (stream.EndOfStream != true)
                {
                    string str = stream.ReadLine();
                    buffer[index] = new char[str.Length + 100];

                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '\0')
                        {
                            buffer[index][i] = ' ';
                        }
                        else
                        {
                            buffer[index][i] = str[i];
                        }
                        
                        Console.Write(buffer[index][i]);

                        if(i == str.Length - 1)
                        {
                            Console.Write("\n");
                        }
                    }

                    index++;
                }
            }

            return buffer;
        }

        internal static char[][] ChangeText(char[][] buffer)
        {
            Console.SetCursorPosition(0, 0);
            int currentLeftPos = 0;
            int currentTopPos = 0;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop != 0)
                        {
                            Console.SetCursorPosition(currentLeftPos, currentTopPos - 1);
                            currentTopPos--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(currentLeftPos, currentTopPos + 1);
                        currentTopPos++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (Console.CursorLeft != 0)
                        {
                            Console.SetCursorPosition(currentLeftPos - 1, currentTopPos);
                            currentLeftPos--;
                        }
                        else
                        {
                            Console.SetCursorPosition(currentLeftPos, currentTopPos++);
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(currentLeftPos + 2, currentTopPos);
                        currentLeftPos++;
                        break;

                    case ConsoleKey.Backspace:
                        Console.Write(' ');
                        if (Console.CursorLeft != 1)
                        {
                            buffer[currentTopPos][--currentLeftPos] = '\0';
                            Console.SetCursorPosition(currentLeftPos, currentTopPos);
                        }
                        break;

                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, ++currentTopPos);
                        currentLeftPos = 0;
                        break;

                    case ConsoleKey.Spacebar:
                        int rowIndex = currentTopPos;
                        int stringIndex = currentLeftPos;

                        if (buffer[rowIndex] is null)
                        {
                            buffer[rowIndex] = new char[100];
                        }

                        int lastIndex = 0;

                        for (int i = stringIndex; i < buffer[rowIndex].Length; i++)
                        {
                            Console.Write($"{buffer[rowIndex][i]}");

                            if (buffer[rowIndex][i] == '\0')
                            {
                                lastIndex = i;
                                break;
                            }
                        }
                        break;

                    case ConsoleKey.Y:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Control) == true)
                        {
                            Console.WriteLine('\n');
                            return buffer;
                        }
                        break;

                    default:
                        if ((int)key.Key >= 65 && (int)key.Key <= 90)
                        {
                            rowIndex = currentTopPos;
                            stringIndex = currentLeftPos;

                            if (buffer[rowIndex] is null)
                            {
                                buffer[rowIndex] = new char[100];
                            }

                            lastIndex = 0;

                            for (int i = stringIndex; i < buffer[rowIndex].Length; i++)
                            {
                                Console.Write($"{buffer[rowIndex][i]}");

                                if (buffer[rowIndex][i] == '\0')
                                {
                                    lastIndex = i;
                                    break;
                                }
                            }

                            Console.SetCursorPosition(stringIndex + 1, currentTopPos);

                            for (int i = lastIndex; i >= stringIndex; i--)
                            {
                                if (i - 1 >= 0)
                                {
                                    buffer[rowIndex][i] = buffer[rowIndex][i - 1];
                                }
                                else
                                {
                                    buffer[rowIndex][i] = key.KeyChar;
                                }
                            }

                            buffer[rowIndex][stringIndex] = key.KeyChar;
                            currentLeftPos++;
                        }
                        break;
                }
            }
        }

        internal static void SaveChanges(char[][] buffer, string name)
        {
            using (StreamWriter stream = new StreamWriter(@$"{Terminal.Path}/{name}", false))
            {
                int rows = buffer.GetUpperBound(0) + 1;

                for (int i = 0; i < rows; i++)
                {
                    if (buffer[i] is not null)
                    {
                        for (int j = 0; j < buffer[i].Length; j++)
                        {
                            stream.Write(buffer[i][j]);
                        }

                        stream.Write('\n');

                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
