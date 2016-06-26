using Xunit;

namespace NodaTime
{    
    public class LocalDateExTests
    {
        [Fact]
        public void WeekDayOfMonth_1stDayOfMOnth_1stWeekdayOfMonth()
        {
            //arrange
            var target = new LocalDate(2016, 06, 01);
            //act
            var result = target.WeekDayOfMonth();
            //assert            
            Assert.Equal(1,result);
        }

        [Fact]
        public void WeekDayOfMonth_LastDayOfMOnth_5thWeekdayOfMonth()
        {
            //arrange
            var target = new LocalDate(2016, 06, 30);
            //act
            var result = target.WeekDayOfMonth();
            //assert            
            Assert.Equal(5, result);
        }

        [Fact]
        public void WeekDayOfMonth_20160626_4thWeekdayOfMonth()
        {
            //arrange
            var target = new LocalDate(2016, 06, 26);
            //act
            var result = target.WeekDayOfMonth();
            //assert            
            Assert.Equal(4, result);
        }

        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_1thWednesdayOf201606_20160601()
        {            
            //act
            var result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016, 06,IsoDayOfWeek.Wednesday, 1);
            //assert            
            Assert.Equal(new LocalDate(2016, 06, 01), result);
        }

        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_5thThursdayOf201606_20160630()
        {
            //act
            var result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016, 06,IsoDayOfWeek.Thursday,5);
            //assert            
            Assert.Equal(new LocalDate(2016, 06, 30), result);
        }

        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_3rdSundayOf201606_20160619()
        {
            //act
            var result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016, 06, IsoDayOfWeek.Sunday, 3);
            //assert            
            Assert.Equal(new LocalDate(2016, 06, 19), result);
        }


        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_LastTuesdayOf201606_20160628()
        {
            //act
            var result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016,06,IsoDayOfWeek.Tuesday,-1);
            //assert            
            Assert.Equal(new LocalDate(2016, 06, 28), result);
        }

        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_2ndLastTuesdayOf201606_20160621()
        {
            //act
            var result = LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016, 06, IsoDayOfWeek.Tuesday, -2);
            //assert            
            Assert.Equal(new LocalDate(2016, 06, 21), result);
        }

        [Fact]
        public void FromMonthWeekDayOfMonthAndWeekDay_DayNumberOutOfScope_InvalidWeekNumberException()
        {
            //we could have "Exists" method, but it is not going to be needed in current Domain logic.

            //act & assert            
            Assert.Throws<InvalidWeekNumberException>(() => LocalDateEx.FromMonthWeekDayOfMonthAndWeekDay(2016, 06, IsoDayOfWeek.Friday, 5));
        }
    }
}
