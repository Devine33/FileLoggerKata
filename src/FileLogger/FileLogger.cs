using System;

namespace FileLogger
{
    public class FileLogger
    {
        private const string Extension = ".txt";
        private const string FileName = "log";
        private readonly IFileProvider _fileProvider;
        private readonly string _date = $"{DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Today.Day}";
        public string FullFileName => FileName + _date + Extension;
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
