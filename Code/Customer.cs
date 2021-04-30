using System;
using System.Collections.Generic;
using System.Linq;
using Code.Preferences;

namespace Code
{
    public class Customer
    {
        public string Name { get; }

        public IMasterPreference MasterPreference { get; }
        public IList<IDaySpecificPreference> DaySpecificPreferences { get; }

        public Customer(string name, IEnumerable<IPreference> preferences)
        {
            Name = name.ToLowerInvariant();
            DaySpecificPreferences = new List<IDaySpecificPreference>();
            
            foreach (var preference in preferences)
            {
                if (preference is EveryDayPreference || preference is DontContactPreference)
                {
                    MasterPreference = (IMasterPreference)preference;
                }
                else
                {
                    DaySpecificPreferences.Add(preference as IDaySpecificPreference);
                }
            }
        }

        public bool WantsPreferencesOn(DateTimeOffset date)
        {
            return 
                MasterPreference?.IsToSendOn(date) ?? 
                DaySpecificPreferences.Any(preference => preference.IsToSendOn(date));
        }
    }
}