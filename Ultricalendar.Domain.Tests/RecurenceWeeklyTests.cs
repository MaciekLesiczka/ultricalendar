using System.Collections.Generic;
using System.Linq;
using NodaTime;
using Xunit;

namespace Ultricalendar.Domain.Tests
{
    public class RecurenceWeeklyTests
    {
        [Fact]
        public void StartDate_EveryOtherWorkday_StartDateIsFirstAfterEnteredOne()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 09), endCondition: new EndCondition.Occurences(2), days: EveryOtherWorkday());
            //act
            var result = target.StartDate;
            //assert
            Assert.Equal(new LocalDate(2016, 06, 10),result); 
        }

        [Fact]
        public void DayOfWeeks_WorkWeeksCollectionIsEmpty_StartDateDeterminesSingleWorkweek()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 09), endCondition: new EndCondition.Occurences(2), days:new HashSet<IsoDayOfWeek>());
            //act
            var result = target.DayOfWeeks;
            //assert
            Assert.Equal(new HashSet<IsoDayOfWeek>{IsoDayOfWeek.Thursday}, result);
        }

        [Fact]
        public void EndDate_EveryOtherWorkday5Times_OcurrencesCountsEverySingleEvent()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), endCondition: new EndCondition.Occurences(5), days: EveryOtherWorkday());
            //act
            var result = target.EndDate;
            //assert
            Assert.Equal(new LocalDate(2016, 06, 20), result); 
        }

        [Fact]
        public void EndDate_EveryOtherWorkday5TimesEveryOtherWeek_StepCountsWeeks()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), endCondition: new EndCondition.Occurences(5), days: EveryOtherWorkday(), step:2);
            //act
            var result = target.EndDate;
            //assert
            Assert.Equal(new LocalDate(2016, 07, 04), result);
        }

        [Fact]
        public void GetEventsSince_FromDateIsBeforeStartDate_StartDate()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 03)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 10), result);
        }

        [Fact]
        public void GetEventsSince_FromDateEqualsStartDate_StartDate()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 10)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 10), result);
        }

        [Fact]
        public void GetEventsSince_FromDateIsBetweenEvents_FirstDateAfter()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 21)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 22), result);
        }

        [Fact]
        public void GetEventsSince_FromDateIsBetweenEventsInTwoWeeks_FirstFromNextWeek()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), step:2, days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 12)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 20), result);
        }

        [Fact]
        public void GetEventsSince_FromDateIsBetweenEventsInTwoWeeksAndInWeekWithoutEvents_FirstDateFromWeekWithEvents()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10), step: 2, days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 15)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 20), result);
        }

        [Fact]
        public void GetEventsSince_FromDateIsInEvent_FirstDateEqualFromDate()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 10),3, days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 27)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 27), result);
        }

        [Fact]
        public void GetEventsSince_FromDateInFirstWeekBetweenEvents_NextDateInFirstWeek()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 08), step: 2, days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 9)).First();
            //assert
            Assert.Equal(new LocalDate(2016, 06, 10), result);
        }


        [Fact]
        public void GetEventsSince_Take4FromInfinite_Returns4ProperDates()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 01), step: 3, days: EveryOtherWorkday());
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 06, 1)).Take(4).ToList();
            //assert
            Assert.Equal(
                new List<LocalDate>
                {
                    new LocalDate(2016, 06, 1),
                    new LocalDate(2016, 06, 3),
                    new LocalDate(2016, 06, 20),
                    new LocalDate(2016, 06, 22)
                }, result);
        }

        [Fact]
        public void GetEventsSince_TakeAllThatLeftFromTheMiddle_RemainingOccurences()
        {
            //arrange
            var target = CreateTarget(new LocalDate(2016, 06, 11), 
                step: 3, 
                days: new HashSet<IsoDayOfWeek> { IsoDayOfWeek.Monday, IsoDayOfWeek.Sunday},
                endCondition:new EndCondition.Occurences(9)
                );
            //act
            var result = target.GetEventsSince(new LocalDate(2016, 08, 18)).ToList();
            //assert
            Assert.Equal(new LocalDate(2016, 09, 4) ,target.EndDate );
            Assert.Equal(
                new List<LocalDate>
                {
                    new LocalDate(2016, 08, 29),
                    new LocalDate(2016, 09, 4)
                }, result);
        }

        private static HashSet<IsoDayOfWeek> EveryOtherWorkday()
        {
            return new HashSet<IsoDayOfWeek>
            {
                IsoDayOfWeek.Monday,
                IsoDayOfWeek.Wednesday,
                IsoDayOfWeek.Friday,
            };
        }
            
        private static Recurrence.Weekly CreateTarget(LocalDate date, int step = 1, EndCondition endCondition = null, HashSet<IsoDayOfWeek> days = null)
        {
            return new Recurrence.Weekly(date,
                step,
                endCondition ?? new EndCondition.Never(),
                days ?? new HashSet<IsoDayOfWeek>());
        }
    }
}