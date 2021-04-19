using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class FileSystemVisitor
    {
        private bool _scan;
        private readonly Predicate<FileSystemItem> _filters;

        public FileSystemVisitor() { }
        public FileSystemVisitor(Predicate<FileSystemItem> filters)
        {
            _filters = filters;
        }

        public IEnumerable<string> FileSystemScan(string path)
        {
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

        private IEnumerable<string> GetFileSystemItem(Func<string, IEnumerable<string>> getItemMethod, string path, string itemName)
        {
            foreach (var searchResult in getItemMethod(path))
            {
                //Event message ($"{Name??} found.");

                FileSystemItem item = new FileSystemItem();
                FileAttributes attributes = File.GetAttributes(searchResult);

                if (!attributes.HasFlag(FileAttributes.Directory))
                {
                    item.Name = Path.GetFileName(searchResult);
                    item.Type = File.ReadAllText(searchResult);
                }

                if (_filters is null)
                {
                    yield return searchResult;
                }
                else if (!_filters(item))
                {
                    continue;
                }

                yield return searchResult;
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

                catch (UnauthorizedAccessException UAEx)
                {
                    // Event message (UAEx.Message);
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
    }
}
