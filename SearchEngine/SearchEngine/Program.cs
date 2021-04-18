using System;
using System.Collections.Generic;
using System.IO;
using FileSystem;

namespace SearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            do
            {
                Console.WriteLine(@"Please enter a correct path (for example c:\Windows):");
                path = @"c:\Disk\English";
                //path = Console.ReadLine();

            } while (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path));

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(GetScanFilters());
            IEnumerable<string> fileSystemItem = fileSystemVisitor.FileSystemScan(path);

            foreach (var item in fileSystemItem)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }

        private static Predicate<FileSystemItem>[] GetScanFilters()
        {
            var filters = new List<Predicate<FileSystemItem>>();

            string searchOption;
            do
            {
                Console.WriteLine("Please enter correct search criteria:" + "\n" +
                                  "1 - Display all files in a directory." + "\n" +
                                  "2 - Use custom search.");
                searchOption = Console.ReadLine();

            } while (!searchOption.Equals("1") && !searchOption.Equals("2"));

            if (searchOption.Equals("2"))
            {
                Console.WriteLine("Please enter your search criteria:" + "\n" +
                                  "Enter type of file:");
                string typeFilter = Console.ReadLine();
                
                if (!string.IsNullOrEmpty(typeFilter))
                {
                    filters.Add(item => item.Type.Contains(typeFilter, StringComparison.InvariantCulture));
                }

                Console.WriteLine("Enter name of file:");
                string nameFilter = Console.ReadLine();

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    filters.Add(item => item.Name.Contains(nameFilter, StringComparison.InvariantCulture));
                }
            }
            
            return filters.ToArray();
        }
    }
}
