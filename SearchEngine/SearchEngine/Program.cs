using System;
using System.Collections.Generic;
using System.IO;
using FileSystem;

namespace SearchEngine
{
    class Program
    {
        private const string TimeOutputFormat = "HH:mm:ss.fff";
        private static (string type, string name) _customSearch;

        static void Main(string[] args)
        {
            string path;
            string repeatSearch;


            do
            {
                Console.WriteLine(@"Please enter a correct path (for example c:\Windows):");
                //path = @"c:\Disk\Books\";
                path = Console.ReadLine();

                while (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                {
                    Console.WriteLine(@"This path does not exist. Please enter a correct path (for example c:\Windows):");
                    path = Console.ReadLine();
                }

                GetSearchOption(ref _customSearch);
                FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(_customSearch);
                IEnumerable<string> fileSystemItem = fileSystemVisitor.FileSystemScan(path);
                fileSystemVisitor.SearchStatus += SearchStatus;

                foreach (var item in fileSystemItem)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nRepeat search one more time? Press Yes: Y, No: N");

                repeatSearch = Console.ReadLine();

            } while (repeatSearch != "N");

            Console.WriteLine("\nAlgorithm finished");

            Console.ReadKey();
        }

        private static void GetSearchOption(ref (string type, string name) customSearch)
        {
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
                                  "type of file:");
                customSearch.type = Console.ReadLine();
                Console.WriteLine("name of file:");
                customSearch.name = Console.ReadLine();
            }
        }

        private static void SearchStatus(object sender, SearchStatusEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.FoundTime.ToString(TimeOutputFormat) + ": " + e.ItemName);
            Console.ResetColor();
        }

        //private static Predicate<FileSystemItem> GetScanFilters()
        //{
        //    Predicate<FileSystemItem> filters = default;
        //    string searchOption;

        //    do
        //    {
        //        Console.WriteLine("Please enter correct search criteria:" + "\n" +
        //                          "1 - Display all files in a directory." + "\n" +
        //                          "2 - Use custom search.");
        //        searchOption = Console.ReadLine();

        //    } while (!searchOption.Equals("1") && !searchOption.Equals("2"));

        //    if (searchOption.Equals("2"))
        //    {
        //        Console.WriteLine("Please enter your search criteria:" + "\n" +
        //                          "type of file:");
        //        string typeFilter = Console.ReadLine();

        //        if (!string.IsNullOrEmpty(typeFilter))
        //        {
        //            filters += item => item.Type.Contains(typeFilter, StringComparison.InvariantCulture);
        //        }

        //        Console.WriteLine("name of file:");
        //        string nameFilter = Console.ReadLine();

        //        if (!string.IsNullOrEmpty(nameFilter))
        //        {
        //            filters += item => item.Name.Contains(nameFilter, StringComparison.InvariantCulture);
        //        }
        //    }

        //    return filters;
        //}
    }
}
