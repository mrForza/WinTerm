using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTerminal
{
    internal class Tree
    {
        public Tree(string startDir)
        {
            StartDir = startDir;
        }

        public string StartDir { get; }

        public bool ShowAll { get; set; } = false;

        public int MaxDepth { get; set; } = int.MaxValue;

        public Action<string> Write { get; set; } = Console.Write;
        public Action<ConsoleColor> SetColor { get; set; } = color => Console.ForegroundColor = color;

        public ConsoleColor DefaultColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor DirColor { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor FileColor { get; set; } = ConsoleColor.DarkGreen;

        private static string[] _nonUsablePathes = new string[] { "C:/$Recycle.Bin", "C:/$WinREAgent", "C:/Documents and Settings", "C:/Config.Msi", "C:/OneDriveTemp", "C:/PerfLogs", "C:/ProgramData", "C:/Recovery" };

        public void Print()
        {
            WriteColored(StartDir, DirColor);
            WriteLine();

            PrintTree(StartDir);
        }

        private void WriteLine(string text = "")
        {
            Write(text + Environment.NewLine);
        }

        private void WriteColored(string text, ConsoleColor color)
        {
            SetColor(color);
            Write(text);
            SetColor(DefaultColor);
        }

        private ConsoleColor GetColor(FileSystemInfo fsItem)
        {
            if (fsItem.IsDirectory())
            {
                return DirColor;
            }
            string ext = Path.GetExtension(fsItem.FullName).ToLower();

            return FileColor;
        }

        private void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, GetColor(fsItem));
        }

        private void PrintTree(string startDir, string prefix = "", int depth = 0)
        {

            if (depth >= MaxDepth)
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(startDir);
            try
            {
                List<FileSystemInfo> fsItems = di.GetFileSystemInfos()
                    .Where(f => ShowAll || !f.Name.StartsWith("."))
                    .OrderBy(f => f.Name)
                    .ToList();

                foreach (var fsItem in fsItems.Take(fsItems.Count - 1))
                {
                    if (_nonUsablePathes.Contains(fsItem.Name))
                    {
                        continue;
                    }

                    Write(prefix + "├── ");
                    WriteName(fsItem);
                    WriteLine();
                    if (fsItem.IsDirectory())
                    {
                        PrintTree(fsItem.FullName, prefix + "│   ", depth + 1);
                    }
                }

                var lastFsItem = fsItems.LastOrDefault();
                if (lastFsItem != null)
                {
                    Write(prefix + "└── ");
                    WriteName(lastFsItem);
                    WriteLine();
                    if (lastFsItem.IsDirectory())
                    {
                        PrintTree(lastFsItem.FullName, prefix + "    ", depth + 1);
                    }

                }
            }
            catch
            {
                Console.WriteLine($"Доступ к папке запрещён!");
            }           
        }
    }

    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
