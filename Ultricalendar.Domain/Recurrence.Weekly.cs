using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NodaTime;

namespace Ultricalendar.Domain
{
    public abstract partial class Recurrence
    {        
        public class Weekly : Recurrence
        {
            private readonly List<IsoDayOfWeek> _dayOfWeeks;

            public Weekly(LocalDate startDate, int step, EndCondition endCondition, HashSet<IsoDayOfWeek> dayOfWeeks)
                : base(FindStartDate(startDate,dayOfWeeks), step, endCondition, Period.FromWeeks)
            {
                foreach (var dayOfWeek in dayOfWeeks)
                {
                    dayOfWeek.EnsureDefined();
                }

                if (dayOfWeeks.Any())
                {
                    _dayOfWeeks = dayOfWeeks.OrderBy(x=> (int)x).ToList();
                }
                else
                {
                    _dayOfWeeks = new List<IsoDayOfWeek>{startDate.IsoDayOfWeek};
                }                                
            }

            public IReadOnlyCollection<IsoDayOfWeek> DayOfWeeks
            {
                get { return new ReadOnlyCollection<IsoDayOfWeek>(_dayOfWeeks); }
            }

            protected override LocalDate GetByOrdinal(int n)
            {
                var weekDayShift = _dayOfWeeks.IndexOf(_startDate.IsoDayOfWeek);

                var weekNumber = (n - weekDayShift)/_dayOfWeeks.Count + 1;
                var dayIndex = (n - weekDayShift)%_dayOfWeeks.Count;

                var weekDay = _dayOfWeeks[dayIndex];

                var eventWeek = _startDate.PlusWeeks(weekNumber*_step);
                var weekYear = eventWeek.WeekYear;
                var weekOfWeekYear = eventWeek.WeekOfWeekYear;

                return LocalDate.FromWeekYearWeekAndDay(weekYear, weekOfWeekYear, weekDay);
            }

            private static LocalDate FindStartDate(LocalDate startDateInput, HashSet<IsoDayOfWeek> dayOfWeeks)
            {
                LocalDate result;
                if (!dayOfWeeks.Any() || dayOfWeeks.Contains(startDateInput.IsoDayOfWeek))
                {
                    result = startDateInput;
                }
                else
                {
                    result = dayOfWeeks.Select(startDateInput.Next).Min();
                }
                return result;
            }
        }      
    }
}