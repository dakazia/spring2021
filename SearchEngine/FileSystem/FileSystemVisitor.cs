using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public sealed class FileSystemVisitor
    {
        private bool _scan;
       private readonly Predicate<FileSystemItem> _filters;
        public event EventHandler<SearchStatusEventArgs> Start;
        public event EventHandler<SearchStatusEventArgs> Finish;
        public event EventHandler<SearchStatusEventArgs> ErrorAppears;
        public event EventHandler<SearchStatusEventArgs> FileFound;
        public event EventHandler<SearchStatusEventArgs> DirectoryFound;
        public event EventHandler<SearchStatusEventArgs> FilteredFileFound;

        public FileSystemVisitor(Predicate<FileSystemItem> filters)
        {
            _filters = filters;
        }

        public IEnumerable<string> FileSystemScan(string path)
        {
            ShowStartEvent("Scan started.");
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

            ShowFinishEvent("Scan finished.");
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
                        ShowFileFoundEvent(($"{itemName} found:"));
                        yield return searchResult;
                    }
                    else if (_filters(item))
                    {
                        ShowFilteredFileFoundEvent($"Filtered {itemName} found:");
                        yield return searchResult;
                    }
                }
                else if (_filters is null)
                {
                    ShowDirectoryFoundEvent($"{itemName} found:");
                    yield return searchResult;
                }
            }
        }

        private void GetDirectories(string path, out IEnumerable<string> iterator)
        {
            var directories = Directory.EnumerateDirectories(path, "*.*");

            iterator= SkipUnauthorizedIterator(directories, path);

            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] subDirs = rootDir.GetDirectories();
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                Console.WriteLine(dirInfo.FullName);
                GetDirectories(dirInfo.FullName, out iterator);
            }
        }

        private IEnumerable<string> GetFiles(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.*");
            return SkipUnauthorizedIterator(files, path);
        }

        private IEnumerable<T> SkipUnauthorizedIterator<T>(IEnumerable<T> iEnumerable, string path)
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
                    ShowErrorAppearsEvent(UAEx.Message);
                }

                catch (Exception e)
                {
                    ShowErrorAppearsEvent(e.Message);
                }

                if (iterator.Current != null)
                {

                  yield return iterator.Current;
                }


            }

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

        private void ShowErrorAppearsEvent(string message)
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
            OnErrorAppears(args);
        }

        private void ShowFileFoundEvent(string message)
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
            OnFileFound(args);
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

        private void OnErrorAppears(SearchStatusEventArgs e)
        {
            EventHandler<SearchStatusEventArgs> handler = ErrorAppears;
            handler?.Invoke(this, e);
        }
    }
}
