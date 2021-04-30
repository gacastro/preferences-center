using System;
using System.Linq;

namespace Code.Preferences
{
    public class DayOfTheMonthPreference: IDaySpecificPreference
    {
        private readonly int[] _chosenDays;

        public DayOfTheMonthPreference(int[] chosenDays)
        {
            _chosenDays = chosenDays;
        }

        public bool IsToSendOn(DateTimeOffset date)
        {
            return _chosenDays.Any(day => day == date.Day);
        }
    }
}