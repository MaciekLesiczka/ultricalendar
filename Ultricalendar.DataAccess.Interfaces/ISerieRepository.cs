﻿using System.Collections.Generic;
using Ultricalendar.Common;
using Ultricalendar.Domain.Entities;

namespace Ultricalendar.DataAccess.Interfaces
{
    public interface ISerieRepository
    {
        List<Series> FindBy(int userId, DateRange dateRange);
    }
}
