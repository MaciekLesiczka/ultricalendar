using System;
using NodaTime;
using OneOf;

namespace Ultricalendar.Domain.Values
{
    public abstract class EndCondition
    {
        private readonly OneOf<Never, Occurences, EndDate> _oneOf;

        protected EndCondition()
        {
            _oneOf = (dynamic) this;
        }

        public class Never : EndCondition { }

        public class Occurences : EndCondition
        {
            private readonly int _number;

            public Occurences(int number)
            {
                if (number < 1)
                {
                    throw new ArgumentException("Step has to be greater than zero");
                }
                _number = number;
            }

            public int Number
            {
                get { return _number; }
            }
        }

        public class EndDate: EndCondition
        {
            private readonly LocalDate _localDate;

            public EndDate(LocalDate localDate)
            {
                _localDate = localDate;
            }

            public LocalDate LocalDate
            {
                get { return _localDate; }
            }
        }

        public OneOf<Never,Occurences,EndDate> OneOf
        {
            get { return _oneOf; }
        }
    }
}