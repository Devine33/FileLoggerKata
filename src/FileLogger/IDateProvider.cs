using System;

namespace FileLogger
{
    public interface IDateProvider
    {
        public bool IsWeekend();
        public DateTime GetPreviousWeekend();
        public DateTime Today { get; }
    }
}