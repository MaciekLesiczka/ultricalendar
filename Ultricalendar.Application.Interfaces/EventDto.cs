using NodaTime;

namespace Ultricalendar.Application.Interfaces
{
    public class EventDto
    {
        public int Id { get; set; }

        public int SerieId { get; set; }

        public LocalDate LocalDate { get; set; }
    } 
}