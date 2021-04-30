using System;

namespace Code.Preferences
{
    public interface IPreference
    {
        bool IsToSendOn(DateTimeOffset date);
    }
}