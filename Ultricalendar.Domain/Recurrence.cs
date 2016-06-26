using System;
using NodaTime;

namespace Ultricalendar.Domain
{
    /// <summary>
    /// Read-only model that represents various scheduling patterns for recurring events.
    /// </summary>
    public abstract partial class Recurrence
    {        
        private readonly LocalDate _startDate;
        private readonly int _step;
        private readonly EndCondition _endCondition;
        private readonly Func<long, Period> _periodFromNumber;

        protected Recurrence(LocalDate startDate, int step, EndCondition endCondition, Func<long,Period> periodFromNumber)
        {     
            if (step < 1)
            {
                throw new ArgumentException("Step has to be greater than zero");
            }

            _startDate = startDate;
            _step = step;
            _endCondition = endCondition;
            _periodFromNumber = periodFromNumber;
        }

        public LocalDate StartDate
        {
            get { return _startDate; }
        }

        public LocalDate? EndDate
        {
            get
            {
                return _endCondition.OneOf.Match(
                    _ => (LocalDate?)null,
                    occurences => GetByOrdinal(occurences.Number),
                    endDate => endDate.LocalDate);
            }
        }

        protected EndCondition EndCondition
        {
            get { return _endCondition; }
        }

        protected int Step
        {
            get { return _step; }
        }

        protected virtual LocalDate GetByOrdinal(int n)
        {
            var delta = (n - 1)*Step;            
            return StartDate + _periodFromNumber(delta);
        }

        public class Daily : Recurrence
        {
            public Daily(LocalDate startDate, int step, EndCondition endCondition) : base(startDate, step, endCondition, Period.FromDays) { }            
        }

        public class Yearly : Recurrence
        {
            public Yearly(LocalDate startDate, int step, EndCondition endCondition) : base(startDate, step, endCondition, Period.FromYears) { }
        }
    }
}