using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using WinTerminal.Commands;

namespace WinTerminal
{
    internal class CommandHandler
    {
        private enum CommandType
        {
            Single = 1,
            Tuple = 2,
            Triple = 3,
            Quad = 4
        }

        private static string[]? _commandExpression;

        private static Dictionary<CommandType, Dictionary<string, Action>> _commandDict = new Dictionary<CommandType, Dictionary<string, Action>>()
        {
            { 
                CommandType.Single, new Dictionary<string, Action>
                {
                    { "/info", (Action)(() => MainCommand.Info()) },
                    { "/help", (Action)(() => MainCommand.Help()) },
                    { "/clear", (Action)(() => Console.Clear()) },
                    { "/exit", (Action)(() => Environment.Exit(0)) }
                }
            },
            {
                CommandType.Tuple, new Dictionary<string, Action>
                {
                    { "/dir contains", (Action)(() => DirectoryCommand.Contains()) },
                    { "/dir back", (Action)(() => DirectoryCommand.Back()) },
                    { "/dir copypath", (Action)(() => Environment.Exit(0)) },
                    { "/dir tree", (Action)(() => DirectoryCommand.DirTree()) },
                    { "/file copypath", (Action)(() => Environment.Exit(0)) }
                }
            },
            {
                CommandType.Triple, new Dictionary<string, Action>
                {
                    { "/file create", (Action)(() => FileCommand.CreateFile()) },
                    { "/file delete", (Action)(() => FileCommand.DeleteFile()) },
                    { "/file open", (Action)(() => FileCommand.Open()) },
                    { "/dir create", (Action)(() => DirectoryCommand.CreateDir()) },
                    { "/dir delete", (Action)(() => DirectoryCommand.DeleteDir()) },
                    { "/dir goto", (Action)(() => DirectoryCommand.GoTo()) },
                    { "/dir exist", (Action)(() => DirectoryCommand.Exist()) }
                }
            }
        };


        internal static void ReadCommands()
        {
            string str = Console.ReadLine();

            if(str is null || str.Length == 0)
            {
                Terminal.DisplayError("Вы не ввели команду");
            }
            else
            {
                _commandExpression = ConvertToCorrectExpression(DeleteExcesseSpaces(str));

                if (_commandExpression is not null)
                {
                    AllocateCommand(_commandExpression);
                }
                else
                {
                    Terminal.DisplayError("Вы ввели некорректную команду");
                }
            }          
        }


        private static int AllocateCommand(string[] commandExpression)
        {
            int counter = 0;

            foreach (CommandType type in _commandDict.Keys)
            {
                int a = (int)type;
                if ((commandExpression.Length + 1) / 2 == (int)type)
                {
                    foreach (string keyWord in _commandDict[type].Keys)
                    {
                        if (CheckKeyWordsExistance(keyWord, _commandExpression) == true)
                        {
                            CallAnotherInitMethods();
                            _commandDict[type][keyWord].Invoke();
                            return 0;
                        }
                    }
                }

                if (counter++ == _commandDict.Count - 1)
                {
                    Terminal.DisplayError("Вы ввели некорректную команду");
                }
            }

            return 0;
        }

        private static void CallAnotherInitMethods()
        {
            FileCommand.InitializeFields(_commandExpression);
            DirectoryCommand.InitializeFields(_commandExpression);
        }       

        private static string DeleteExcesseSpaces(string inputString)
        {
            bool wordEntry = false;
            bool spaceEntry = false;
            string modifierString = "";

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == ' ')
                {
                    if (spaceEntry == false && wordEntry == true)
                    {
                        modifierString += " ";
                        spaceEntry = true;
                    }
                }
                else
                {
                    modifierString += inputString[i];
                    wordEntry = true;
                    spaceEntry = false;
                }
            }

            string outputString = "";

            if (modifierString[modifierString.Length - 1] == ' ')
            {
                for (int i = 0; i < modifierString.Length - 1; i++)
                {
                    outputString += modifierString[i];
                }
            }
            else
            {
                outputString = modifierString;
            }

            return outputString;
        }

        private static string DetermineQuatations(string[] inputExpression, out int firstIndex)
        {
            string name = "";
            firstIndex = -1;
            int lastIndex = -1;

            for (int i = 0; i < inputExpression.Length; i++)
            {
                if(firstIndex == -1 && inputExpression[i].Contains(','))
                {
                    firstIndex = i;
                    break;
                }
            }

            for (int i = inputExpression.Length - 1; i >= 0; i--)
            {
                if (lastIndex == -1 && inputExpression[i].Contains('\''))
                {
                    lastIndex = i;
                    break;
                }
            }

            for (int i = firstIndex; i <= lastIndex; i++)
            {
                name += inputExpression[i];
            }

            return name;
        }

        private static string[] ConvertToCorrectExpression(string inputString)
        {
            string[] modifierExpression = new string[2 * inputString.Split(' ').Length - 1];
            string subString = "";
            int j = 0;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == ' ')
                {
                    modifierExpression[j] = subString;

                    if (j != modifierExpression.Length - 1)
                    {
                        modifierExpression[++j] = " ";
                    }

                    j++;

                    subString = "";                 
                }
                else
                {
                    subString += inputString[i];

                    if (j == modifierExpression.Length - 1 && i == inputString.Length - 1)
                    {
                        modifierExpression[j] = subString;
                    }
                    
                }

            }

            if(inputString.Contains(',') == true && inputString.Contains('\'') == true)
            {
                string name = DetermineQuatations(modifierExpression, out int firstIndex);
                string[] outputExpression = new string[firstIndex + 1];

                for (int i = 0; i < firstIndex + 1; i++)
                {
                    outputExpression[i] = modifierExpression[i];

                    if (i == firstIndex)
                    {
                        outputExpression[i] = name;
                    }
                }

                return outputExpression;
            }
            else
            {
                return modifierExpression;
            }            
        }

        private static bool CheckKeyWordsExistance(string keyWord, string[] commandExpression)
        {
            string strCommand = "";

            for (int i = 0; i < commandExpression.Length; i++)
            {
                strCommand += $"{commandExpression[i]}";
            }

            int lastIndexOfKeyWord = keyWord.Length - 1;

            if (strCommand.Contains(keyWord) && (keyWord.Length == strCommand.Length || strCommand[lastIndexOfKeyWord + 1] == ' '))
            {
                return true;
            }
            
            return false;
        }
    }
}