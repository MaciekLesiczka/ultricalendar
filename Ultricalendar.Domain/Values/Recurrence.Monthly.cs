using NodaTime;

namespace Ultricalendar.Domain.Values
{
    public abstract partial class Recurrence
    {
        public class Monthly : Recurrence
        {
            private readonly RepeatBy _repeatBy;

            public enum RepeatBy
            {
                MonthDay,
                WeekDay
            }

            public Monthly(LocalDate startDate, int step, EndCondition endCondition, RepeatBy repeatBy)
                : base(startDate, step, endCondition, Period.FromMonths)
            {
                _repeatBy = repeatBy;
            }

            protected override LocalDate GetByOrdinal(int n)
            {
                var result = base.GetByOrdinal(n);
                if (_repeatBy == RepeatBy.WeekDay)
                {
                    var weekDayOfMonth = _startDate.WeekDayOfMonth();
                    weekDayOfMonth = weekDayOfMonth == 5 ? -1 : weekDayOfMonth;
                    result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(result.Year, result.Month, _startDate.IsoDayOfWeek, weekDayOfMonth);
                }

                return result;
            }

            protected override LocalDate FirstEventSince(LocalDate date)
            {
                throw new System.NotImplementedException();
            }
        }  
    }
}