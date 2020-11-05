namespace FileLogger
{
    public interface IFileProvider
    {
        public void Append(string message);
        public bool FileExists();
        public void CreateFile();
    }
}
