using System;

namespace Code.Helpers
{
    [Flags]
    public enum DaysOfTheWeek: ushort
    {
        None = 0,
        Sunday = 1 << 0,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6
    }
    
    public static class EnumExtensions
    {
        public static DaysOfTheWeek ToFlag(this DayOfWeek dayOfWeek)
        {
            var mask = 1 << (int)dayOfWeek;
            return (DaysOfTheWeek)Enum.ToObject(typeof(DaysOfTheWeek), mask);
        }
    }
}