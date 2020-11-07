using System;

namespace FileLogger
{
    public class FileLogger
    {
        private readonly IFileProvider _fileProvider;
        private readonly IDateProvider _dateProvider;

        private readonly string _fileName = "log";
        private readonly string _logExtension = ".txt";
 
        private string WeekendFile => $"weekend.txt";

        public FileLogger(IFileProvider provider, IDateProvider iDateProvider = null)
        {
            _fileProvider = provider;
            _dateProvider = iDateProvider;
        }

        public void Log(string message)
        {

            if (DoesPreviousWeekendLogExist())
            {
                RenamePreviousLog();
            }


            var fileName = GetFileName();

            if (!_fileProvider.FileExists(fileName))
            {
                _fileProvider.CreateFile(fileName);
            }

            _fileProvider.Append(fileName, message);
        }

        private bool IsWeekend()
        {
            return _dateProvider.IsWeekend();
        }


        private void RenamePreviousLog()
        {
            DateTime fileDate = _fileProvider.GetCreationDate(WeekendFile);
            _fileProvider.Rename(WeekendFile,$"weekend-{fileDate:yyyyMMdd}.txt");
        }

        private bool DoesPreviousWeekendLogExist()
        {
            return _fileProvider.FileExists(WeekendFile);
        }

        private string FileName => $"{_fileName}{_dateProvider.Today:yyyyMMdd}{_logExtension}";

        private string GetFileName()
        {
            return IsWeekend() ? WeekendFile : FileName;
        }
    }
}