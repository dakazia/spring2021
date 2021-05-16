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

        public static void Main()
        {
            string repeatSearch;

            do
            {
                Console.WriteLine(@"Please enter a correct path (for example c:\Windows):");
                var path = Console.ReadLine();

                while (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                {
                    Console.WriteLine(@"This path does not exist. Please enter a correct path (for example c:\Windows):");
                    path = Console.ReadLine();
                }

                GetSearchOption(ref _customSearch);

                FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(GetScanFilters());
                IEnumerable<string> fileSystemItem = fileSystemVisitor.FileSystemScan(path);
                fileSystemVisitor.Start += Start;
                fileSystemVisitor.DirectoryFound += DirectoryFound;
                fileSystemVisitor.FileFound += FileFound;
                fileSystemVisitor.FilteredFileFound += FilteredFileFound;
                fileSystemVisitor.Finish += Finish;

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

        private static Predicate<FileSystemItem> GetScanFilters()
        {
            Predicate<FileSystemItem> filters = default;

            if (!string.IsNullOrEmpty(_customSearch.type))
            {
                filters += item => item.Type.Contains(_customSearch.type);
            }

            if (!string.IsNullOrEmpty(_customSearch.name))
            {
                filters += item => item.Name.Contains(_customSearch.name);
            }

            return filters;
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
                                  " - type of file:");
                customSearch.type = Console.ReadLine();
                Console.WriteLine(" - name of file:");
                customSearch.name = Console.ReadLine();
            }
        }

        private static void FileFound(object sender, SearchStatusEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(e.FoundTime.ToString(TimeOutputFormat) + ": " + e.ItemName);

            if (e.ItemName.Contains("SkipCriteria"))
            {
                e.ShouldSkipItem = true;
                Console.WriteLine($"File : { e.ItemName } is skipped");
            }

            if (e.ItemName.Contains("stop"))
            {
                e.ShouldAbortSearch = true;
                Console.WriteLine($" Search will be abort due to file: {e.ItemName}");
            }

            Console.ResetColor();
        }

        private static void Start(object sender, EventArgs eventArgs)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Scan started.");
            Console.ResetColor();
        }

        private static void Finish (object sender, EventArgs eventArgs)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Scan finished.");
            Console.ResetColor();
        }

        private static void FilteredFileFound(object sender, SearchStatusEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(e.FoundTime.ToString(TimeOutputFormat) + ": " + e.ItemName);
            Console.ResetColor();
        }

        private static void DirectoryFound(object sender, SearchStatusEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(e.FoundTime.ToString(TimeOutputFormat) + ": " + e.ItemName);

            if (e.ItemName.Contains("SkipCriteria"))
            {
                e.ShouldSkipItem = true;
                Console.WriteLine($"Directory : { e.ItemName } is skipped");
            }

            if (e.ItemName.Contains("AbortCriteria"))
            {
                e.ShouldAbortSearch = true;
                Console.WriteLine($" Search will be abort due to directory: {e.ItemName}");
            }

            Console.ResetColor();
        }
    }
}
