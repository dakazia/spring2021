using System;

namespace FileSystem
{
    public class SearchStatusEventArgs : EventArgs
    {
        public string ItemName { get; set; }

        public DateTime FoundTime { get; set; }

        public bool ShouldAbortSearch { get; set; }

        public bool ShouldSkipItem { get; set; }
    }
}
