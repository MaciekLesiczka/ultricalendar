using System.Collections.Generic;
using Ultricalendar.Common;

namespace Ultricalendar.Application.Interfaces
{
    public interface IEventService
    {
        List<EventDto> FindUserEvents(int userId, DateRange dateRange);
    }
}