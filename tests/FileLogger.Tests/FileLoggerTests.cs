using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NUnit.Framework;

namespace FileLogger.Tests
{
    public class Tests
    {
        private Mock<IFileProvider> _fileProviderMock;
        private Mock<IDateProvider> _dateProviderMock;
        private FileLogger _logger;
        private string _testMessage = "test";
        private readonly DateTime Today = new DateTime(2020, 11, 6);
        private string FileName => $"log{Today:yyyyMMdd}.txt";

        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _dateProviderMock = new Mock<IDateProvider>();
            _dateProviderMock.Setup(x => x.IsWeekend()).Returns(false);
            _fileProviderMock.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>()));
            _logger = new FileLogger(_fileProviderMock.Object, _dateProviderMock.Object);
        }

        [Test]
        public void FileDoesntExistHaveMessageAppended()
        {
            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.Append(FileName, _testMessage), Times.Once);
            _fileProviderMock.Verify(x => x.FileExists(FileName), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(FileName), Times.Once);
        }

        [Test]
        public void FileExistsHaveMessageAppended()
        {
            _fileProviderMock.Setup(x => x.FileExists(FileName)).Returns(true);

            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.Append(FileName, _testMessage), Times.Once);
            _fileProviderMock.Verify(x => x.FileExists(FileName), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(FileName), Times.Never);
        }


        [Test]
        public void FileDosentExistOnWeekendHaveMessageAppended()
        {
            var weekendFilename = "weekend.txt";
            _dateProviderMock.Setup(x => x.IsWeekend()).Returns(true);
            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.Append(weekendFilename, _testMessage), Times.Once);
            _fileProviderMock.Verify(x => x.FileExists(weekendFilename), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(weekendFilename), Times.Once);
        }
    }
}