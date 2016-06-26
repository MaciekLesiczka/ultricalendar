using System.Collections.Generic;
using Ultricalendar.Common;
using Ultricalendar.Domain.Entities;

namespace Ultricalendar.DataAccess.Interfaces
{
    public interface IEventRepository
    {
        List<Event> FindSingleEvents(int userId, DateRange dateRange);
    }
}