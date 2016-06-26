using System;

namespace NodaTime
{
    public static class LocalDateEx
    {
        /// <summary>
        /// Returns a nummber from range [1,5]
        /// </summary>
        public static int WeekDayOfMonth(this LocalDate date)
        {
            return date.Day / 7 + 1;
        }

        /// <summary>
        /// Similar one to LocalDate.FromWeekYearWeekAndDay
        /// </summary>
        /// <returns></returns>
        public static LocalDate FromMonthWeekDayOfMonthAndWeekDay(int year, int month, IsoDayOfWeek dayOfWeek, int weekOfMonth)
        {
            LocalDate result;
            dayOfWeek.EnsureDefined();
            if (weekOfMonth > 0)
            {
                var lastDayPrevMonth = new LocalDate(year, month, 1) - Period.FromDays(1);

                result = lastDayPrevMonth.Next(dayOfWeek);
                for (int i = 1; i < weekOfMonth; i++)
                {
                    result = result.Next(dayOfWeek);
                }    
            }
            else if( weekOfMonth <0 )
            {
                var firstDayOfNextMonth = new LocalDate(year, month, 1).PlusMonths(1);
                result = firstDayOfNextMonth.Previous(dayOfWeek);
                for (int i = -1; i > weekOfMonth; i--)
                {
                    result = result.Previous(dayOfWeek);
                }    
            }
            else
            {
                throw new ArgumentException("weekOfMonth must be non-zero value", "weekOfMonth");
            }

            if (result.Month != month || result.Year != year)
            {
                throw new InvalidWeekNumberException(year, month, dayOfWeek, weekOfMonth);
            }
            
            return result;
        }        
    }
}

