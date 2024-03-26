using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinTerminal.Commands
{
    internal class DirectoryCommand
    {
        private static string _name;

        internal static void InitializeFields(string[] commandExpression)
        {
            if(commandExpression.Length == 5)
            {
                _name = commandExpression[4];
            }
            else
            {
                _name = null;
            }
        }

        internal static void CreateDir()
        {
            if(_name.Contains('.') == false)
            {
                Directory.CreateDirectory($"{Terminal.CheckBasePath()}/{_name}");
                Terminal.DisplaySuccessMessage($"Директория {_name} создана");
            }
            else
            {
                Terminal.DisplayError("Директория не может содержать расширение");
            }
        }

        internal static void DeleteDir()
        {
            if (_name.Contains('.') == false)
            {
                if(Directory.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
                {
                    Directory.Delete($"{Terminal.CheckBasePath()}/{_name}");
                    Terminal.DisplaySuccessMessage($"Директория {_name} удалена");
                }
                else
                {
                    Terminal.DisplayError("Такой директории не существует!");
                }               
            }
            else
            {
                Terminal.DisplayError("Директория не может содержать расширение");
            }
        }

        internal static void Exist()
        {
            if (Directory.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
            {
                DisplayExistance(true);
            }
            else
            {
                if (File.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
                {
                    DisplayExistance(true);
                }
                else
                {
                    DisplayExistance(false);
                }
            }
        }      

        internal static void Contains()
        {
            DirectoryInfo directories = new DirectoryInfo($"{Terminal.CheckBasePath()}/");
            FileInfo[] files = directories.GetFiles(".");

            Console.ForegroundColor = ConsoleColor.Blue;
            int i = 0;

            Console.WriteLine("\nДиректории:\n");
            foreach (DirectoryInfo dir in directories.GetDirectories(""))
            {
                Console.WriteLine($"{dir.Name}");             
            }
            
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n\n\nФайлы:\n");
            i = 0;

            foreach (FileInfo file in files)
            {
                var a = file.Length / (int)Math.Pow(1024, 1);

                if (file.Length / (int)Math.Pow(1024, 3) > 0)
                {
                    Console.WriteLine($"{file.Name}   {file.Length / (int)Math.Pow(1024, 3)} Гб");
                }
                else if (file.Length / (int)Math.Pow(1024, 2) > 0)
                {
                    Console.WriteLine($"{file.Name}   {file.Length / (int)Math.Pow(1024, 2)} Мб");
                }
                else if (file.Length / 1024 > 0)
                {
                    Console.WriteLine($"{file.Name}   {file.Length / 1024} Кб");
                }
                else
                {
                    Console.WriteLine($"{file.Name}   {file.Length} б");
                }
                
            }

            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void DirTree()
        {
            Tree tree;

            if ($"{Terminal.CheckBasePath()}" == "C:")
            {
                tree = new Tree($"{Terminal.CheckBasePath()}/");
            }
            else
            {
                tree = new Tree($"{Terminal.CheckBasePath()}");
            }
            
            tree.Print();
        }      

        internal static void GoTo()
        {
            if (Directory.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
            {
                Terminal.Path += $"/{_name}";
                Terminal.DisplaySuccessMessage($"Вы переместились в директорию {_name}\n");
            }
            else
            {
                Terminal.DisplayError("Вы ввели неверное название директории\n");
            }
        }

        internal static void Back()
        {
            int sleshCounter = 0;
            string path = Terminal.CheckBasePath();

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    sleshCounter++;
                }
            }

            string localPath = "";

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {                  
                    
                    if (sleshCounter == 1)
                    {
                        break;
                    }

                    localPath += path[i];
                    sleshCounter--;
                }
                else
                {
                    localPath += path[i];
                }
            }

            Terminal.Path = localPath;
        }

        private static void DisplayExistance(bool existance)
        {
            if (existance == true)
            {
                Terminal.DisplaySuccessMessage("Yes");
            }
            else
            {
                Terminal.DisplayError("No");
            }
        }
    }
}
