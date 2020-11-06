using Moq;
using NUnit.Framework;

namespace FileLogger.Tests
{
    public class Tests
    {
        private Mock<IFileProvider> _fileProviderMock;
        private FileLogger _logger;
        private string _testMessage = "test";
        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _fileProviderMock.Setup(x => x.Append(It.IsAny<string>()));
            _fileProviderMock.Setup(x => x.CreateFile());
            _logger = new FileLogger(_fileProviderMock.Object);
        }

        [Test]
        public void FileShouldExist()
        {
            _fileProviderMock.Setup(x => x.FileExists()).Returns(true);

            _logger.Log("test");
            _fileProviderMock.Verify(x=>x.FileExists(),Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(), Times.Never);
        }

        [Test]
        public void FileShouldHaveMessageAppended()
        {
            _fileProviderMock.Setup(x => x.FileExists()).Returns(true);

            _logger.Log("test");
            _fileProviderMock.Verify(x => x.FileExists(), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(), Times.Never);
            _fileProviderMock.Verify(x=>x.Append(_testMessage),Times.Once);
        }
    }
}