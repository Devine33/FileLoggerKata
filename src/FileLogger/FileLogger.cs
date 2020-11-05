using System;

namespace FileLogger
{
    public class FileLogger
    {
        private const string _extension = ".txt";
        private readonly IFileProvider _fileProvider;
        private readonly string _date = $"{DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Today.Day}";

        public FileLogger(IFileProvider provider)
        {
            _fileProvider = provider;
        }
        public void Log(string message)
        {

            if (!_fileProvider.FileExists())
            {
                _fileProvider.CreateFile();
            }

            _fileProvider.Append(message);
        }
    }
}
