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
                var weekDayShift =  _dayOfWeeks.IndexOf(StartDate.IsoDayOfWeek);

                var weekNumber = (n + weekDayShift - 1) / _dayOfWeeks.Count;
                var dayIndex = (n + weekDayShift - 1) % _dayOfWeeks.Count;

                var weekDay = _dayOfWeeks[dayIndex];

                var eventWeek = StartDate.PlusWeeks(weekNumber*Step);
                var weekYear = eventWeek.WeekYear;
                var weekOfWeekYear = eventWeek.WeekOfWeekYear;

                return LocalDate.FromWeekYearWeekAndDay(weekYear, weekOfWeekYear, weekDay);
            }

            protected override LocalDate FirstEventSince(LocalDate date)
            {
                if (StartDate >= date)
                {
                    return StartDate;
                }
                //what is the whole-week-distance between recurrence start date and given date?
                var sinceDateWeekOrdinal = Period.Between(StartDate, LocalDate.FromWeekYearWeekAndDay(date.WeekYear, date.WeekOfWeekYear, StartDate.IsoDayOfWeek), PeriodUnits.Weeks).Weeks;

                if (sinceDateWeekOrdinal%Step == 0)//we are in a week, when events occur, let's check there are still any to return from it.
                {
                    var nextEventInCurrentWeek = _dayOfWeeks.Where(x => x >= date.IsoDayOfWeek).ToList();
                    if (nextEventInCurrentWeek.Any())
                    {
                        return LocalDate.FromWeekYearWeekAndDay(date.WeekYear, date.WeekOfWeekYear, nextEventInCurrentWeek.Min());
                    }
                }

                var nextWeekWithEvents = sinceDateWeekOrdinal + Step - sinceDateWeekOrdinal % Step;

                return (
                    StartDate + Period.FromWeeks(nextWeekWithEvents)
                    ).Previous(IsoDayOfWeek.Sunday).Next(_dayOfWeeks.First());
            }

            protected override LocalDate Next(LocalDate eventDate)
            {
                //we have a bunch of O(n) lookups here.. but performance shouldn't be an issue since _dayOfWeeks is really small list.
                var indexOf = _dayOfWeeks.IndexOf(eventDate.IsoDayOfWeek);

                if (indexOf < _dayOfWeeks.Count - 1) //there are still some events in this week
                {
                    return eventDate.Next(_dayOfWeeks[indexOf + 1]);
                }
                else//That was the last one. Move to another week and start with first specified day.
                {
                    return eventDate.PlusWeeks(Step).Previous(_dayOfWeeks[0]);
                }
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