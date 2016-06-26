using NodaTime;

namespace Ultricalendar.Domain.Entities
{
    public class Event
    {
        private readonly int _id;
        private readonly LocalDate _date;

        public Event( int id, LocalDate date)
        {
            _id = id;
            _date = date;
        }

        public int Id
        {
            get { return _id; }
        }

        public LocalDate Date
        {
            get { return _date; }
        }
    }
}