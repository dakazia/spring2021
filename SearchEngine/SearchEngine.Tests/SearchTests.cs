using System;
using System.Collections.Generic;
using System.IO;
using FileSystem;
using NUnit.Framework;
using System.Linq;

namespace SearchEngine.Tests
{
    [TestFixture]
    public class SearchTests
    {

        private static string testFolderPath = Path.GetTempPath();

        private string testPath = Path.Combine(testFolderPath, "Test");

        private const int CountFilteredFile = 1;

        private List<string> expectedCollection = new List<string>();

        private static string MakePath(params string[] tokens) 
        {
            string fullpath = "";

            foreach (string token in tokens)
            {
                fullpath = Path.Combine(fullpath, token);
            }

            return fullpath;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            Directory.CreateDirectory(testFolderPath);

            string[] testDirs =
            {
                    MakePath(testFolderPath, "Test", "directory1"),
                    MakePath(testFolderPath, "Test", "directory1", "directory2"),
                    MakePath(testFolderPath, "Test", "directory1", "directory2", "directory3")
                };

            foreach (string dir in testDirs)
            {
                expectedCollection.Add(dir);
                Directory.CreateDirectory(dir);
            }

            string[] testFiles =
            {
                    MakePath(testFolderPath, "Test", "directory1", "file1.txt"),
                    MakePath(testFolderPath, "Test", "directory1", "skip.txt"),
                    MakePath(testFolderPath, "Test", "directory1", "directory2", "stop.txt"),
                    MakePath(testFolderPath, "Test", "directory1", "directory2", "file4.txt")
                };
            foreach (string file in testFiles)
            {
                expectedCollection.Add(file);
                FileStream str = File.Create(file);
                str.Close();
            }

            expectedCollection.Sort();
        }


        [Test]
        public void FileSystemScan_WithFilter()
        {
            Predicate<FileSystemItem> filters = default;
            filters += item => item.Type.Contains("txt");
            filters += item => item.Name.Contains("file1");

            FileSystemVisitor visitor = new FileSystemVisitor(filters);
            IEnumerable<string> collection = visitor.FileSystemScan(testPath);
            List<string> collectionSort = collection.ToList();
            collectionSort.Sort();

            Assert.AreEqual(
                CountFilteredFile, collectionSort.Count);
        }

        [Test]
        public void FileSystemScan_WithOutFilter()
        {
            FileSystemVisitor visitor = new FileSystemVisitor(null);
            IEnumerable<string> collection = visitor.FileSystemScan(testPath);
            List<string> collectionSort = collection.ToList();
            collectionSort.Sort();

            Assert.AreEqual(
                expectedCollection, collectionSort);
        }

        [Test]
        public void FileSystemScan_PathIsNull_ThrowArgumentNullException()
        {
            FileSystemVisitor visitor = new FileSystemVisitor(null);
            IEnumerable<string> collection = visitor.FileSystemScan(null);

            Assert.Throws<ArgumentNullException>(() => collection.ToList());
        }

        [Test]
        public void FileSystemScan_ItemIsExcludedFromResult()
        {
            FileSystemVisitor visitor = new FileSystemVisitor(null);
            IEnumerable<string> collection = visitor.FileSystemScan(testPath);
            visitor.FileFound += FileFound;

            Assert.False(collection.Contains("skip"));
        }

        [Test]
        public void FileSystemScan_SearchIsStopped()
        {
            FileSystemVisitor visitor = new FileSystemVisitor(null);
            IEnumerable<string> collection = visitor.FileSystemScan(testPath);
            visitor.FileFound += FileFound;

            Assert.IsTrue(expectedCollection.Count() > collection.Count());
        }

        private static void FileFound(object sender, SearchStatusEventArgs e)
        {
            if (e.ItemName.Contains("skip"))
            {
                e.ShouldSkipItem = true;
            }

            if (e.ItemName.Contains("stop"))
            {
                e.ShouldAbortSearch = true;
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(
                testFolderPath + "Test", true);
        }
    }
}


