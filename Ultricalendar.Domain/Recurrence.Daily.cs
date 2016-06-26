using System.Collections.Generic;
using NodaTime;

namespace Ultricalendar.Domain
{
    public abstract partial class Recurrence
    {
        public class Daily : Recurrence
        {
            public Daily(LocalDate startDate, int step, EndCondition endCondition) : base(startDate, step, endCondition, Period.FromDays) { }

            public IEnumerable<LocalDate> GetEventsFrom(LocalDate date)
            {
                var eventDate = FirstEventSince(date);
                yield return eventDate;
            }

            protected override LocalDate FirstEventSince(LocalDate date)
            {
                if (StartDate >= date)
                {
                    return StartDate;
                }

                var period = Period.Between(StartDate, date);
                if (period.Days % Step == 0)
                {
                    return date;
                }

                return date + Period.FromDays(Step - period.Days % Step);
            }
        } 
    }
}