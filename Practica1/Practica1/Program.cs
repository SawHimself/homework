using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Practica1;
using System.IO.Compression;

namespace HelloApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();

                // создаем каталог для файла
                string path = @"C:\SomeDir2\";
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                Console.WriteLine("Введите строку для записи в файл:");
                string text = Console.ReadLine();

                // запись в файл
                using (FileStream fstream = new FileStream($@"{path}\note.txt", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }

                // чтение из файла
                using (FileStream fstream = File.OpenRead($@"{path}\note.txt"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine($"Текст из файла: {textFromFile}");
                }

                FileInfo fileInf = new FileInfo($@"{path}\note.txt");
                Console.WriteLine("Удалить файл? NO - нет");
                string DeleteMe = Console.ReadLine();
                if (!(DeleteMe == "NO"))
                {
                    if (fileInf.Exists)
                    {
                        fileInf.Delete();
                        Console.WriteLine("File delete");
                    }
                    else
                    {
                        Console.WriteLine("Ops!");
                    }
                }
                else
                {
                    Console.WriteLine("Ok");
                }
                //json
                Console.WriteLine("Введите имя и возраст для сериализаци данных");
                string? DataUserName = Console.ReadLine();
                if (DataUserName == null)
                {
                    DataUserName = "Null";
                }
                int DataUserAge = Convert.ToInt32(Console.ReadLine());

                // сохранение данных
                using (FileStream fs = new FileStream(@"C:\Учебники\Операционные системы\Practica1\Practica1\user.json", FileMode.OpenOrCreate))
                {
                    Person tom = new Person() { Name = DataUserName, Age = DataUserAge };
                    await JsonSerializer.SerializeAsync<Person>(fs, tom);
                    Console.WriteLine("Данные сохранены");
                }

                // чтение данных
                using (FileStream fs = new FileStream(@"C:\Учебники\Операционные системы\Practica1\Practica1\user.json", FileMode.OpenOrCreate))
                {
                    if (fs != null)
                    {
                        Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
                        Console.WriteLine($"Имя: {restoredPerson.Name}  Возраст: {restoredPerson.Age}");
                    }
                }
                //Удаление файла
                fileInf = new FileInfo(@"C:\Учебники\Операционные системы\Practica1\Practica1\user.json");
                fileInf.Delete();


                //xml
                Console.WriteLine("Введите имя и возраст для XML файла");
                DataUserName = Console.ReadLine();
                if (DataUserName == null)
                {
                    DataUserName = "Null";
                }
                DataUserAge = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Погнали с XML");
                XDocument xdoc = new XDocument(new XElement("Данные",
                    new XElement("Пользователь",
                        new XElement("Имя", DataUserName),
                        new XElement("Возраст", DataUserAge ))));
                xdoc.Save(@"C:\Учебники\Операционные системы\Practica1\Practica1\Data.xml");
                //Удаление файла
                fileInf = new FileInfo(@"C:\Учебники\Операционные системы\Practica1\Practica1\Data.xml");
                fileInf.Delete();

                //Архив @"C:\Учебники\Операционные системы\Practica1\Practica1\DataFromZ\"
                string sourceFolder = "NULL";
                Console.WriteLine("Выберите файл который нужно архивировать");
                Console.WriteLine("1 - Text1.txt");
                Console.WriteLine("2 - Text2.txt");
                int Text = Convert.ToInt32(Console.ReadLine());
                if(Text == 1)
                {
                    sourceFolder = @"C:\Учебники\Операционные системы\Practica1\Practica1\DataFromZ\Text1.txt";
                }
                else if(Text == 2)
                {
                    sourceFolder = @"C:\Учебники\Операционные системы\Practica1\Practica1\DataFromZ\Text2.txt"; // исходная папка
                }
                string zipFile = @"C:\Учебники\Операционные системы\Practica1\Practica1\DataFromZ.zip"; // сжатый файл
                string targetFolder = "D://newtest"; // папка, куда распаковывается файл
                if (sourceFolder != "NULL")
                {
                    ZipFile.CreateFromDirectory(sourceFolder, zipFile);
                }
                Console.WriteLine($"Папка {sourceFolder} архивирована в файл {zipFile}");
                ZipFile.ExtractToDirectory(zipFile, targetFolder);

                Console.WriteLine($"Файл {zipFile} распакован в папку {targetFolder}");
                Console.ReadLine();
            }
        }
    }
}