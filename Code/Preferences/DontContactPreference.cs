using System;

namespace Code.Preferences
{
    public class DontContactPreference: IMasterPreference
    {
        public bool IsToSendOn(DateTimeOffset date)
        {
            return false;
        }
    }
}