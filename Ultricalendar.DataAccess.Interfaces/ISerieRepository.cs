using System.Collections.Generic;
using Ultricalendar.Domain;

namespace Ultricalendar.DataAccess.Interfaces
{
    public interface ISerieRepository
    {
        List<Series> FindBy(UserId userId, DateRange dateRange);
    }
}
