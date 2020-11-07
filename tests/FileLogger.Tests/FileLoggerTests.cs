using System;
using Moq;
using NUnit.Framework;

namespace FileLogger.Tests
{
    public class Tests
    {
        private Mock<IFileProvider> _fileProviderMock;
        private Mock<IDateProvider> _dateProviderMock;
        private string FileName => $"log{DateTime.Today:yyyyMMdd}.txt";
        private FileLogger _logger;
        private readonly string _testMessage = "test";
        private readonly DateTime _saturday = new DateTime(2020, 11, 7);
        private static readonly DateTime Sunday = new DateTime(2020, 11, 8);


        [SetUp]
        public void Setup()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _dateProviderMock = new Mock<IDateProvider>();
            _dateProviderMock.Setup(x => x.IsWeekend()).Returns(false);
            _dateProviderMock.Setup(x => x.Today).Returns(DateTime.Today);
            _fileProviderMock.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>()));
            _logger = new FileLogger(_fileProviderMock.Object, _dateProviderMock.Object);
        }

        [Test]
        public void FileDoesntExistHaveMessageAppended()
        {
            _fileProviderMock.Setup(x => x.FileExists(FileName)).Returns(false);
            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.FileExists(FileName), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(FileName), Times.Once);
            _fileProviderMock.Verify(x => x.Append(FileName, _testMessage), Times.Once);
        }

        [Test]
        public void FileExistsHaveMessageAppended()
        {
            _fileProviderMock.Setup(x => x.FileExists(FileName)).Returns(true);

            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.FileExists(FileName), Times.Once);
            _fileProviderMock.Verify(x => x.CreateFile(FileName), Times.Never);
            _fileProviderMock.Verify(x => x.Append(FileName, _testMessage), Times.Once);
        }

        [Test]
        public void FileDosentExistOnWeekendHaveMessageAppended()
        {
            var weekendFilename = "weekend.txt";
            _dateProviderMock.Setup(x => x.IsWeekend()).Returns(true);
            _logger.Log(_testMessage);
            _fileProviderMock.Verify(x => x.FileExists(weekendFilename), Times.AtLeastOnce);
            _fileProviderMock.Verify(x => x.CreateFile(weekendFilename), Times.Once);
            _fileProviderMock.Verify(x => x.Append(weekendFilename, _testMessage), Times.Once);
        }

        [Test]
        public void WeekendLogFileRotatesWhenNewLogStartsOnSaturday()
        {
            var lastSunday = _saturday.AddDays(-6);
            var secondOfLastSunday = lastSunday.Add(new TimeSpan(23, 59, 59));
            var expectedLogFile = "weekend.txt";
            var expectedArchivedLogFile = $"weekend-{lastSunday:yyyyMMdd}.txt";
            _dateProviderMock.Setup(dp => dp.GetPreviousWeekend()).Returns(lastSunday);
            _fileProviderMock.Setup(x => x.FileExists(expectedLogFile)).Returns(true);
            _fileProviderMock.Setup(fs => fs.GetCreationDate(expectedLogFile)).Returns(secondOfLastSunday);

            _logger.Log(_testMessage);

            _fileProviderMock.Verify(fs => fs.Rename(expectedLogFile, expectedArchivedLogFile), Times.Once);
            _fileProviderMock.Verify(fs => fs.GetCreationDate(expectedLogFile), Times.AtLeastOnce);
        }

        [Test]
        public void WeekendLogFileRenamedOnWeekendIfPreviousExists()
        {
            var lastSunday = Sunday.AddDays(-7);
            var secondOfLastSunday = lastSunday.Add(new TimeSpan(23, 59, 59));
            var expectedLogFile = "weekend.txt";
            var expectedArchivedLogFile = $"weekend-{lastSunday:yyyyMMdd}.txt";

            _dateProviderMock.Setup(x => x.Today).Returns(Sunday);
            _fileProviderMock.Setup(x => x.FileExists(expectedLogFile)).Returns(true);
            _fileProviderMock.Setup(x => x.GetCreationDate(expectedLogFile)).Returns(secondOfLastSunday);

            _logger.Log(_testMessage);

            _fileProviderMock.Verify(x => x.Rename(expectedLogFile, expectedArchivedLogFile), Times.Once);
            _fileProviderMock.Verify(x => x.GetCreationDate(expectedLogFile), Times.AtLeastOnce);
        }
    }
}