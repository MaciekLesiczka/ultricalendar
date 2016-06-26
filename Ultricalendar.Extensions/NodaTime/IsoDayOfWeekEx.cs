using System.ComponentModel;

namespace NodaTime
{
    public static class IsoDayOfWeekEx
    {
        public static void EnsureDefined(this IsoDayOfWeek dayOfWeek)
        {
            if (dayOfWeek == IsoDayOfWeek.None)
            {
                throw new InvalidEnumArgumentException("None value for IsoDayOfWeek is invalid.");
            }
        }
    }
}