using System;

namespace FileLogger
{
    public class FileLogger
    {
        private const string Extension = ".txt";
        private const string FileName = "log";
        private readonly IFileProvider _fileProvider;
        private DateTime _date = DateTime.Today;
        public string FullFileName => $"{FileName}{_date:yyyyMMdd}{Extension}";
        public FileLogger(IFileProvider provider)
        {
            _fileProvider = provider;
        }
        public void Log(string message)
        {

            if (!_fileProvider.FileExists(FullFileName))
            {
                _fileProvider.CreateFile(FullFileName);
            }

            _fileProvider.Append(message);
        }


    }
}
