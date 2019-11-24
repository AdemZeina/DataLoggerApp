using System.Collections.Generic;
using ServiceStack;

namespace DataLoggerApp.Models
{
    public class FileTraceModel
    {
        public string FileName { get; set; }

        public int TotalCount { get; set; }
        public int FilesItemCount { get; set; }

        public List<FileItemModel> FileItemsList { get; set; }

    }

    public class FileItemModel
    {
        public string FileItemName { get; set; }

        public int FileItemCount { get; set; }

        public FileStatus FileState { get; set; }
    }

    public  enum FileStatus {
    Empty,
    HavingSize,
    Full
    }
}