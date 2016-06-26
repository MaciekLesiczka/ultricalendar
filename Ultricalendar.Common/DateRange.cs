using NodaTime;

namespace Ultricalendar.Common
{
    public class DateRange
    {
        private readonly LocalDate _from;
        private readonly LocalDate _to;

        public DateRange(LocalDate from, LocalDate to)
        {
            _from = @from;
            _to = to;
        }

        public LocalDate From
        {
            get { return _from; }
        }

        public LocalDate To
        {
            get { return _to; }
        }

        public bool Contains(LocalDate date)
        {
            return date >= From && date <= To;
        }
    }
}