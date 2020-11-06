namespace FileLogger
{
    public interface IFileProvider
    {
        public void Append(string message);
        public bool FileExists(string file);
        public void CreateFile(string file);
    }
}
