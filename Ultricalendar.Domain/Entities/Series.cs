﻿using System.Collections.Generic;
using System.Linq;
using NodaTime;
using Ultricalendar.Common;
using Ultricalendar.Domain.Values;

namespace Ultricalendar.Domain.Entities
{
    public class Series
    {
        private readonly int _id;
        private readonly Recurrence _recurrence;

        private readonly Dictionary<LocalDate, Event> _shifts = new Dictionary<LocalDate, Event>();

        public Series(int id, Recurrence recurrence)
        {
            _id = id;
            _recurrence = recurrence;
        }

        public int Id
        {
            get { return _id; }
        }

        public IEnumerable<Event> GetEvents(DateRange dateRange)
        {                                    
            foreach (var localDate in _recurrence.GetEventsSince(dateRange.From)
                                                 .TakeWhile( dateRange.Contains))
            {
                if (!_shifts.ContainsKey(localDate))
                {
                    yield return new Event(0, localDate);
                }
            }

            foreach (var @event in _shifts.Values)
            {
                if (dateRange.Contains(@event.Date))
                {
                    yield return @event;                    
                }                    
            }
        }
    }
}
