using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        if (args.Length < 2)
        {
            ShowHelp();
            return;
        }

        string directoryPath = args[0];
        string filePattern = args[1];

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Указаний каталог не існує.");
            return;
        }

        DirectoryInfo directory = new DirectoryInfo(directoryPath);
        FileInfo[] files = directory.GetFiles(filePattern);

        if (files.Length == 0)
        {
            Console.WriteLine("У вказаному каталозі немає файлів, що відповідають шаблону.");
            return;
        }

        DateTime latestDate = DateTime.MinValue;
        foreach (FileInfo file in files)
        {
            if (file.Attributes.HasFlag(FileAttributes.Hidden) ||
                file.Attributes.HasFlag(FileAttributes.ReadOnly) ||
                file.Attributes.HasFlag(FileAttributes.Archive))
            {
                if (file.LastWriteTime > latestDate)
                {
                    latestDate = file.LastWriteTime;
                }
            }
        }
        foreach (FileInfo file in files)
        {
            if (file.LastWriteTime == latestDate)
            {
                string destinationPath = Path.Combine(directory.FullName, "Synced", file.Name);
                file.CopyTo(destinationPath, true);
            }
        }
        Console.WriteLine("Синхронізація завершена.");
    }

    static void ShowHelp()
    {
        Console.WriteLine("Синхронізація цифрових файлів у двох каталогах за датою.");
        Console.WriteLine("Приклад введення данних: <каталог> <шаблон файлу>");

        Console.WriteLine("Введіть шлях до каталогу:");
        string directoryPath = Console.ReadLine();

        Console.WriteLine("Введіть шаблон файлу (наприклад, *.txt):");
        string filePattern = Console.ReadLine();

        Main(new string[] { directoryPath, filePattern });
    }
}
