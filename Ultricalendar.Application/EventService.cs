using System.Collections.Generic;
using System.Linq;
using Ultricalendar.Application.Interfaces;
using Ultricalendar.Common;
using Ultricalendar.DataAccess.Interfaces;
using Ultricalendar.Domain.Values;

namespace Ultricalendar.Application
{
    public class EventService
    {
        private readonly ISerieRepository _serieRepository;

        public EventService(ISerieRepository serieRepository)
        {
            _serieRepository = serieRepository;
        }

        public List<EventDto> FindUserEvents(UserId userId, DateRange dateRange)
        {
            return _serieRepository
                .FindBy(userId, dateRange)
                .SelectMany(s => s.GetEvents(dateRange).Select(e => new EventDto
                {
                    Id = e.Id,
                    SerieId = s.Id,
                    LocalDate = e.Date
                }))
                .ToList();
        }
    }
}
