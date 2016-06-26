using System;
using NodaTime;

namespace Ultricalendar.Domain
{
    public abstract partial class Recurrence
    {
        public class Yearly : Recurrence
        {
            public Yearly(LocalDate startDate, int step, EndCondition endCondition) : base(startDate, step, endCondition, Period.FromYears) { }

            protected override LocalDate FirstEventSince(LocalDate date)
            {
                throw new NotImplementedException();
            }
        }
    }
}