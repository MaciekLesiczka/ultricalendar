using System.Collections.Generic;
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