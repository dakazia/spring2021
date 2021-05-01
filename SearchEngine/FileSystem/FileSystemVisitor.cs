using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class FileSystemVisitor
    {
        private int _node = 10; // number of inner catalogs
        private readonly Predicate<FileSystemItem> _filters;
        public event EventHandler<SearchStatusEventArgs> Start;
        public event EventHandler<SearchStatusEventArgs> Finish;
        public event EventHandler<SearchStatusEventArgs> FileFound;
        public event EventHandler<SearchStatusEventArgs> DirectoryFound;
        public event EventHandler<SearchStatusEventArgs> FilteredFileFound;

        public FileSystemVisitor(Predicate<FileSystemItem> filters)
        {
            _filters = filters;
        }

        public IEnumerable<string> FileSystemScan(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            ShowStartEvent("Scan started.");

            foreach (var directory in GetFileSystemItem(GetElements, path))
            {
                yield return directory;
            }

            ShowFinishEvent("Scan finished.");
        }

        private IEnumerable<string> GetFileSystemItem(Func<string, IEnumerable<string>> getItemMethod, string path)
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
                        var args = new SearchStatusEventArgs()
                        {
                            ItemName = "File found:" + " " + item.Name,
                            FoundTime = DateTime.Now
                        };

                        OnFileFound(args);

                        if (args.ShouldAbortSearch)
                        {
                            yield break;
                        }
                        if (!args.ShouldSkipItem)
                        {
                            yield return searchResult;
                        }
                        
                    }
                    else if (_filters(item))
                    {
                        ShowFilteredFileFoundEvent($"Filtered file found:");

                        yield return searchResult;
                    }
                }
                else if (_filters is null)
                {
                    ShowDirectoryFoundEvent($"Directory found:");
                    yield return searchResult;
                }
            }
        }

        private IEnumerable<string> GetElements(string path)
        {
            var files = GetFiles(path);
            foreach (var file in files)
            {
                yield return file;
            }

            var directories = GetDirectories(path);

            foreach (var directory in directories)
            {
                yield return directory;

                bool continueSearch = _node > 0;

                if (continueSearch)
                {
                    foreach (var item in GetElements(directory))
                    {
                        yield return item;
                        _node--;
                    }
                }
            }
        }

        public virtual IEnumerable<string> GetFiles(string path)
        {
            return Directory.EnumerateFiles(path, "*.*");
        }

        public virtual IEnumerable<string> GetDirectories(string path)
        {
            return Directory.EnumerateDirectories(path, "*.*");
        }

        private void ShowStartEvent(string message)
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

            OnStart(args);
        }

        private void ShowFinishEvent(string message)
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

            OnFinish(args);
        }

        private void ShowDirectoryFoundEvent(string message)
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

            OnDirectoryFound(args);
        }
        
        private void ShowFilteredFileFoundEvent(string message)
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

            OnFilteredFileFound(args);
        }
        private void OnStart(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = Start;
            handler?.Invoke(this, e);
        }

        private void OnFinish(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = Finish;
            handler?.Invoke(this, e);
        }

        private void OnDirectoryFound(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = DirectoryFound;
            handler?.Invoke(this, e);
        }

        private void OnFileFound(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = FileFound;
            handler?.Invoke(this, e);
        }

        private void OnFilteredFileFound(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = FilteredFileFound;
            handler?.Invoke(this, e);
        }
    }
}
