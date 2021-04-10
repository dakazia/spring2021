using System;
using System.Collections.Generic;
using FileSystem;

namespace SearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string path = @"c:\Disk\English";

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor();
            IEnumerable<string> fileSystemItem = fileSystemVisitor.FileSystemScan(path);
        }
    }
}
