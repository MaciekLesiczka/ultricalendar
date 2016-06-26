using NodaTime;
using Xunit;

namespace Ultricalendar.Domain.Tests
{
    public class RecurrenceMonthlyTests
    {
        [Fact]
        public void EndDate_5thSunday_LastSundayInNextMonth()
        {
            //arrange
            var monthly = CreateTarget(new LocalDate(2016, 7, 31));
            //act
            var result = monthly.EndDate;
            //assert           
            Assert.Equal(new LocalDate(2016, 8, 28), result);
        }

        [Fact]
        public void EndDate_4thWeekday5Isavailable_4SundayInNextMonth()
        {
            //arrange
            var monthly = CreateTarget(new LocalDate(2016, 7, 24));
            //act
            var result = monthly.EndDate;
            //assert           
            Assert.Equal(new LocalDate(2016, 8, 28), result);
        }

        [Fact]
        public void EndDate_LastAnd4thWeekdayWhen5SundaysIsAvailableInNextMonth_4SundayInNextMonth()
        {
            //arrange
            var monthly = CreateTarget(new LocalDate(2016, 6, 26));
            //act
            var result = monthly.EndDate;
            //assert           
            Assert.Equal(new LocalDate(2016, 7, 24), result);
        }

        private static Recurrence.Monthly CreateTarget(LocalDate startDate)
        {
            return new Recurrence.Monthly(startDate, 1, new EndCondition.Occurences(2), Recurrence.Monthly.RepeatBy.WeekDay);
        }
    }
}