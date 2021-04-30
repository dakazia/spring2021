using System;
using System.Collections.Generic;
using System.IO;
using FileSystem;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace SearchEngine.Tests
{

    public class SearchTests
    {

        [TestFixture]
        public class DirectoryTest
        {
            private readonly string testFolderPath =
                Path.GetTempPath();

            private List<string> expectedDirs =
                new List<string>();

            private List<string> expectedFiles =
                new List<string>();

            private List<string> expectedCollection =
                new List<string>();

            static private string MakePath(
                params string[] tokens)
            {
                string fullpath = "";
                foreach (string token in tokens)
                {
                    fullpath = Path.Combine(fullpath, token);
                }

                return fullpath;
            }

            [SetUp]
            public void Setup()
            {
                Directory.CreateDirectory(testFolderPath);

                string[] testDirs =
                {
                    MakePath(testFolderPath, "Test", "dir1"),
                    MakePath(testFolderPath, "Test", "dir1", "dir2"),
                    MakePath(testFolderPath, "Test", "dir1", "dir2", "dir3")
                };

                foreach (string dir in testDirs)
                {
                    expectedDirs.Add(dir);
                    Directory.CreateDirectory(dir);
                }

                expectedDirs.Sort();
             
                string[] testFiles =
                {
                    MakePath(testFolderPath, "Test", "dir1", "file1.txt"),
                    MakePath(testFolderPath, "Test", "dir1", "file2.txt"),
                    MakePath(testFolderPath, "Test", "dir1", "dir2", "file3.txt"),
                    MakePath(testFolderPath, "Test", "dir1", "dir2", "file4.txt")
                };
                foreach (string file in testFiles)
                {
                    expectedFiles.Add(file);
                    FileStream str = File.Create(file);
                    str.Close();
                }

                expectedFiles.Sort();
            }

            [Test]
            public void VisitDirectory()
            {
                string testPath = Path.Combine(
                    testFolderPath, "Test");
                Predicate<FileSystemItem> filters = default;

                filters += item => item.Type.Contains("file");
                filters += item => item.Name.Contains("file");

                FileSystemVisitor visitor = new FileSystemVisitor(filters);
                IEnumerable<string> collection = visitor.FileSystemScan(testPath);
                List<string> collectionSort = collection.ToList();
                collectionSort.Sort();

                Assert.AreEqual(
                    expectedFiles, collectionSort);

            }

            [TearDown]
            public void TearDown()
            {
                Directory.Delete(
                    testFolderPath + "Test", true);
            }
        }

    }
}

