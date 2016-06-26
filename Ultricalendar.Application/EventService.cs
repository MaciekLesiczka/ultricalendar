using System.Collections.Generic;
using System.Linq;
using Ultricalendar.Application.Interfaces;
using Ultricalendar.Common;
using Ultricalendar.DataAccess.Interfaces;

namespace Ultricalendar.Application
{
    internal class EventService : IEventService
    {
        private readonly ISerieRepository _serieRepository;
        private readonly IEventRepository _eventRepository;

        public EventService(ISerieRepository serieRepository, IEventRepository eventRepository)
        {
            _serieRepository = serieRepository;
            _eventRepository = eventRepository;
        }

        public List<EventDto> FindUserEvents(int userId, DateRange dateRange)
        {
            return _serieRepository
                .FindBy(userId, dateRange)
                .SelectMany(s => s.GetEvents(dateRange).Select(e => new EventDto
                {
                    Id = e.Id,
                    SerieId = s.Id,
                    LocalDate = e.Date
                }))
                .Concat(_eventRepository.FindSingleEvents(userId, dateRange)
                .Select(e => new EventDto
                {
                    Id = e.Id,

                    LocalDate = e.Date
                }))
                .ToList();
        }
    }
}
