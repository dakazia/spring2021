using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class FileSystemVisitor
    {
        private bool _scan;
        private readonly Predicate<FileSystemItem>[] _filters;

        public FileSystemVisitor()
        {
            _filters = new Predicate<FileSystemItem>[] { };
        }

        public IEnumerable<string> FileSystemScan(string path)
        {
            if (!Directory.Exists(path))
            {
                //Event message ("Path do not exists.");

                yield break;
            }

            //Event message ("Scan started.");
            _scan = true;

            foreach (var directory in GetFileSystemItem(GetDirectories, path, "directory"))
            {
                yield return directory;
            }

            foreach (var file in GetFileSystemItem(GetFiles, path, "file"))
            {
                yield return file;
            }

            // Event message ("Scan finished.");
        }

        public void StopScan()
        {
            _scan = false;
        }

        private IEnumerable<string> GetFileSystemItem(Func<string, IEnumerable<string>> getItemMethod, string path, string itemName)
        {
            foreach (var searchResult in getItemMethod(path))
            {
                //Event message ($"{Name??} found.");

                if (_filters.Length == 0)
                {
                    yield return searchResult;
                }
                else if (UseFilter(searchResult))
                {
                    // Event message ($"Filtered {itemName} found.");
                    yield return searchResult;
                }
            }
        }

        private IEnumerable<string> GetDirectories(string path)
        {
            var directories = Directory.EnumerateDirectories(path, "*.*", SearchOption.AllDirectories);
            return SkipUnauthorizedIterator(directories);
        }

        private IEnumerable<string> GetFiles(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
            return SkipUnauthorizedIterator(files);
        }

        private IEnumerable<T> SkipUnauthorizedIterator<T>(IEnumerable<T> iEnumerable)
        {
            var iterator = iEnumerable.GetEnumerator();

            while (_scan)
            {
                try
                {
                    if (!iterator.MoveNext())
                    {
                        break;
                    }
                }

                catch (UnauthorizedAccessException uAEx)
                {
                    // Event message (uAEx.Message);
                }

                catch (Exception e)
                {
                    // Event message (e.Message);
                }

                if (iterator.Current != null)
                {
                    yield return iterator.Current;
                }
            }
        }

        private bool UseFilter(string searchResult)
        {
            throw new NotImplementedException();
        }
    }

}
