using System.Linq;
using NodaTime;
using Ultricalendar.Domain.Values;
using Xunit;

namespace Ultricalendar.Domain.Tests.Values
{
    public class RecurrenceDailyTests
    {
        [Fact]
        public void GetEventsFrom_FromDateIsBeforeStartDate_StartDate()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 09));
            //act
            var result = target.GetEventsFrom(new LocalDate(2016,06,03)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 09), result);
        }

        [Fact]
        public void GetEventsFrom_FromDateIsBetweenEvents_FirstDateAfter()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 09),2);
            //act
            var result = target.GetEventsFrom(new LocalDate(2016, 06, 12)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 13), result);
        }

        [Fact]
        public void GetEventsFrom_FromDateIsInEvent_FirstDateEqualFromDate()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 09), 2);
            //act
            var result = target.GetEventsFrom(new LocalDate(2016, 06, 15)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 15), result);
        }

        private static Recurrence.Daily CreateTarget(LocalDate date, int step = 1, EndCondition endCondition = null)
        {
            return new Recurrence.Daily(date,step, endCondition ?? new EndCondition.Never());
        }
    }
}