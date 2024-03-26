using WinTerminal.Editor.ImageEditor;
using WinTerminal.Editor;

namespace WinTerminal.Commands
{
    internal class FileCommand
    {
        private static string? _name;

        private static string _flag;

        private static char[] _nonUsableSymbols = new char[] 
        {
            '\\', '/', ':', '*', '?', '<', '>', '|', '"'
        };

        private static string[] _immageExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".bmp"
        };


        internal static void InitializeFields(string[] commandExpression)
        {
            if (commandExpression.Length == 5)
            {
                _name = commandExpression[4];               
            }
            else if (commandExpression.Length == 7)
            {
                _name = commandExpression[4];
                _flag = commandExpression[6];
            }
            else
            {
                _name = null;
                _flag = null;
            }
        }

        internal static void CreateFile()
        {
            if (CheckFileNameExistance(_name) == true && CheckFileNameCorrectness(_name) == true)
            {
                try
                {
                    File.Create($"{Terminal.CheckBasePath()}/{_name}").Close();
                    

                    Terminal.DisplaySuccessMessage($"Файл {_name} создан\n");
                }
                catch
                {
                    Terminal.DisplayWarning("Файл занят другим процессом");
                }               
            }   
        }

        internal static void DeleteFile()
        { 
            if (CheckFileNameExistance(_name) == true && CheckFileNameCorrectness(_name) == true)
            {
                if (File.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
                {
                    try
                    {
                        File.Delete($"{Terminal.CheckBasePath()}/{_name}");

                        Terminal.DisplaySuccessMessage("Файл удалён\n");
                    }
                    catch
                    {
                        Terminal.DisplayWarning("Файл занят другим процессом");
                    }
                }
                else
                {
                    Terminal.DisplayError("Файл не найден\n");
                }                
            }
        }

        internal static void Open()
        {
            if (CheckFileNameExistance(_name) == true && CheckFileNameCorrectness(_name) == true)
            {
                if (File.Exists($"{Terminal.CheckBasePath()}/{_name}") == true)
                {
                    for (int i = 0; i < _immageExtensions.Length; i++)
                    {
                        if (_name.Contains(_immageExtensions[i]) == true)
                        {
                            ImageDisplayer.DisplayImage(_name);
                            break;
                        }

                        if (i == _immageExtensions.Length - 1)
                        {
                            try
                            {
                                char[][] buffer = TextEditor.DisplayFileContent(_name);
                                buffer = TextEditor.ChangeText(buffer);

                                TextEditor.SaveChanges(buffer, _name);
                            }
                            catch
                            {
                                Terminal.DisplayWarning("Файл занят другим процессом");
                            }
                        }                            
                    }                    
                }
                else
                {
                    Terminal.DisplayError("Файл не найден\n");
                }
            }
        }


        private static bool CheckFileNameExistance(string inputName)
        {
            if(inputName is null)
            {
                Terminal.DisplayError("Вы не ввели имя файла\n");
                return false;
            }

            return true;
        }

        private static bool CheckFileNameCorrectness(string inputName)
        {
            if (inputName.Contains(',') == true && inputName.Contains('\'') == true)
            {
                if(CheckExstensionCorretness(inputName) == true && CheckQuatationCorrectness(inputName) == true)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return CheckExstensionCorretness(inputName);
            }

        }       
        
        private static bool CheckQuatationCorrectness(string inputName)
        {
            if (inputName[0] == ',' && inputName[inputName.Length - 1] == '\'')
            {
                _name = DeleteQuatations(inputName);
                return true;
            }
            else
            {
                Terminal.DisplayError("Вы ввели некорректное имя файла");
                return false;
            }
        }

        private static bool CheckExstensionCorretness(string inputName)
        {
            bool dotExistance = false;

            if (inputName.Contains('.') == true)
            {
                for (int i = 0; i < inputName.Length; i++)
                {
                    if (_nonUsableSymbols.Contains(inputName[i]) == true)
                    {
                        Terminal.DisplayError($"Файл содержит недопусттимый символ: {inputName[i]}");
                        return false;
                    }
                    else
                    {
                        if (dotExistance == false)
                        {
                            if (inputName[i] == '.')
                            {
                                if (i == inputName.Length - 1)
                                {
                                    Terminal.DisplayError("Имя файла содержит некорректное расширение\n");
                                    return false;
                                }
                                else
                                {
                                    dotExistance = true;
                                }
                            }                            
                        }
                        else
                        {
                            if (inputName[i] == '.' || inputName[i] == ' ')
                            {
                                Terminal.DisplayError("Имя файла содержит некорректное расширение\n");
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                Terminal.DisplayError("Имя файла не содержит расширение");
                return false;
            }

            return true;
        }

        private static string DeleteQuatations(string inputName)
        {
            string outputName = "";

            for (int i = 1; i < inputName.Length - 1; i++)
            {
                outputName += inputName[i];
            }

            return outputName;
        }
    }
}