using System.Collections.Generic;
using Ultricalendar.Common;
using Ultricalendar.Domain.Entities;
using Ultricalendar.Domain.Values;

namespace Ultricalendar.DataAccess.Interfaces
{
    public interface ISerieRepository
    {
        List<Series> FindBy(UserId userId, DateRange dateRange);
    }
}
