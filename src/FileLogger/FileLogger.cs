using System;

namespace FileLogger
{
    public class FileLogger
    {
        private IFileProvider _fileProvider;
        private IDateProvider _dateProvider;
        private string _fileName = "log";
        private string _logExtension = ".txt";
        private DateTime Today => DateTime.Today;
        private string WeekendFile => $"weekend.txt";

        public FileLogger(IFileProvider provider, IDateProvider iDateProvider = null)
        {
            _fileProvider = provider;
            _dateProvider = iDateProvider;
        }

        public void Log(string message)
        {
            var fileName = GetFileName();

            if (!_fileProvider.FileExists(fileName))
            {
                _fileProvider.CreateFile(fileName);
            }

            _fileProvider.Append(fileName, message);
        }

        public bool IsWeekend()
        {
            return _dateProvider.IsWeekend();
        }


        private string FileName => $"{_fileName}{Today:yyyyMMdd}{_logExtension}";

        public string GetFileName()
        {
            return IsWeekend() ? WeekendFile : FileName;
        }
    }

    public interface IDateProvider
    {
        public bool IsWeekend();
    }
}