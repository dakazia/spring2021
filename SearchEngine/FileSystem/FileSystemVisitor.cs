using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class FileSystemVisitor
    {
        private bool _scan;
        private readonly Predicate<FileSystemItem> _filters;
        public event EventHandler<SearchStatusEventArgs> SearchStatus;

        public FileSystemVisitor(Predicate<FileSystemItem> filters)
        {
            _filters = filters;
        }

        public IEnumerable<string> FileSystemScan(string path)
        {
            ShowSearchStatusEvent("Scan started.");
            _scan = true;

            foreach (var directory in GetFileSystemItem(GetDirectories, path, "Directory"))
            {
                yield return directory;

            }
            foreach (var file in GetFileSystemItem(GetFiles, path, "File"))
            {
                yield return file;
            }
            _scan = false;

            ShowSearchStatusEvent("Scan finished.");
        }

        private IEnumerable<string> GetFileSystemItem(Func<string, IEnumerable<string>> getItemMethod, string path, string itemName)
        {
            foreach (var searchResult in getItemMethod(path))
            {
                FileSystemItem item = new FileSystemItem();
                FileAttributes attributes = File.GetAttributes(searchResult);

                if (!attributes.HasFlag(FileAttributes.Directory))
                {
                    item.Name = Path.GetFileName(searchResult);
                    item.Type = File.ReadAllText(searchResult);

                    if (_filters is null)
                    {
                        ShowSearchStatusEvent(($"{itemName} found:"));
                        yield return searchResult;
                    }
                    else if (_filters(item))
                    {
                        ShowSearchStatusEvent($"Filtered {itemName} found:");
                        yield return searchResult;
                    }
                }
                else if (_filters is null)
                {
                    ShowSearchStatusEvent($"{itemName} found:");
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

                catch (UnauthorizedAccessException UAEx)
                {
                    ShowSearchStatusEvent(UAEx.Message);
                }

                catch (Exception e)
                {
                    ShowSearchStatusEvent(e.Message);
                }

                if (iterator.Current != null)
                {
                    yield return iterator.Current;
                }
            }
        }

        private void ShowSearchStatusEvent(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace", nameof(message));
            }

            var args = new SearchStatusEventArgs()
            {
                ItemName = message,
                FoundTime = DateTime.Now
            };
            OnEntryScanned(args);
        }

        protected virtual void OnEntryScanned(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = SearchStatus;
            handler?.Invoke(this, e);
        }
    }
}
