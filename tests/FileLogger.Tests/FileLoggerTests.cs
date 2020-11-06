using System;
using Moq;
using NUnit.Framework;

namespace FileLogger.Tests
{
    public class Tests
    {
        private Mock<IFileProvider> _fileProviderMock;
        private FileLogger _logger;
        private string _testMessage = "test";
        private DateTime DefaultToday => DateTime.Today;
        private string Filename => $"log{DefaultToday:yyyy:MM:dd}.txt";

        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _fileProviderMock.Setup(x => x.Append(It.IsAny<string>()));
            _fileProviderMock.Setup(x => x.CreateFile(Filename));
            _logger = new FileLogger(_fileProviderMock.Object);
        }

        [Test]
        public void FileShouldExistHaveMessageAppended()
        {
            _fileProviderMock.Setup(x => x.FileExists(Filename)).Returns(true);

            _logger.Log("test");
            _fileProviderMock.Verify(x => x.FileExists(Filename), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(Filename), Times.Never);
            _fileProviderMock.Verify(x => x.Append(_testMessage), Times.Once);
        }

        [Test]
        public void FileShouldBeCreatedIfNotExistAndAppendMessage()
        {
            _fileProviderMock.Setup(x => x.FileExists(Filename)).Returns(false);

            _logger.Log("test");
            _fileProviderMock.Verify(x => x.FileExists(Filename), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(Filename), Times.Once);
            _fileProviderMock.Verify(x => x.Append(_testMessage), Times.Once);
        }
    }
}