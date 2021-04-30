using System;

namespace Code.Preferences
{
    public class EveryDayPreference: IMasterPreference
    {
        public bool IsToSendOn(DateTimeOffset date)
        {
            return true;
        }
    }
}