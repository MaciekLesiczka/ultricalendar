using System;
using System.Collections.Generic;
using NodaTime;

namespace Ultricalendar.Domain.Values
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

        /// <summary>
        /// Is null if recurrence is ininite.
        /// </summary>
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

        public IEnumerable<LocalDate> GetEventsSince(LocalDate date)
        {
            var eventDate = FirstEventSince(date);
            var endDate = EndDate;

            Func<LocalDate, bool> outSideRange = e => endDate.HasValue && endDate < e;
            if (outSideRange(eventDate))
            {
                yield break;
            }
            yield return eventDate;

            while (true)
            {
                eventDate = Next(eventDate);
                if (outSideRange(eventDate))
                {
                    break;
                }
                yield return eventDate;
            }
        }

        protected virtual LocalDate GetByOrdinal(int n)
        {
            var delta = (n - 1)*Step;            
            return StartDate + _periodFromNumber(delta);
        }

        protected virtual LocalDate Next(LocalDate date)
        {
            return date + _periodFromNumber(Step);
        }

        protected abstract LocalDate FirstEventSince(LocalDate date);      
    }
}