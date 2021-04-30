using System;
using Code.Helpers;

namespace Code.Preferences
{
    public class DayOfTheWeekPreference: IDaySpecificPreference
    {
        private readonly DaysOfTheWeek _chosenDays;

        public DayOfTheWeekPreference(DaysOfTheWeek chosenDays)
        {
            _chosenDays = chosenDays;
        }

        public bool IsToSendOn(DateTimeOffset date)
        {
            var dayOfTheWeek = date.DayOfWeek.ToFlag();
            return _chosenDays.HasFlag(dayOfTheWeek);
        }
    }
}