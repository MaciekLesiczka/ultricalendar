using System;
using System.Runtime.Serialization;

namespace NodaTime
{
    [Serializable]
    public class InvalidWeekNumberException : Exception
    {
        private readonly int _year;
        private readonly int _month;
        private readonly IsoDayOfWeek _dayOfWeek;
        private readonly int _weekOfMonth;

        public InvalidWeekNumberException(int year, int month, IsoDayOfWeek dayOfWeek, int weekOfMonth)
        {
            _year = year;
            _month = month;
            _dayOfWeek = dayOfWeek;
            _weekOfMonth = weekOfMonth;
        }

        public InvalidWeekNumberException(string message) : base(message)
        {
        }

        public InvalidWeekNumberException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidWeekNumberException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int Year
        {
            get { return _year; }
        }

        public int Month
        {
            get { return _month; }
        }

        public IsoDayOfWeek DayOfWeek
        {
            get { return _dayOfWeek; }
        }

        public int WeekOfMonth
        {
            get { return _weekOfMonth; }
        }
    }
}