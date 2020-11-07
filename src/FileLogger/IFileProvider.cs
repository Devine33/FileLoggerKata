using System;

namespace FileLogger
{
    public interface IFileProvider
    {
        public void Append(string file,string message);
        public bool FileExists(string file);
        public void CreateFile(string file);
        public void Rename(string file,string newFileName);
        DateTime GetCreationDate(string weekendFile);
    }
}
