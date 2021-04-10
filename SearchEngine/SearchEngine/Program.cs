using System;
using

namespace SearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var path = args[0];
            //var path = @"c:\Disk\English";

            var fileSystemVisitor = new FileSystemVisitor();
            var fileSystemEntry = fileSystemVisitor.FileSystemScan(path);
        }
    }
}
