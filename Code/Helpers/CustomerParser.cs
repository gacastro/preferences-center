using System;
using System.Collections.Generic;
using Code.Preferences;

namespace Code.Helpers
{
    public class CustomerParser
    {
        public IList<Customer> Customers { get; }
        
        private readonly string[] _lineElements;

        public CustomerParser(IEnumerable<string> lines)
        {
            Customers = new List<Customer>();

            foreach (var line in lines)
            {
                _lineElements = line.Split('|');
                var customerName = _lineElements[0];
                var customerPreferences = new List<IPreference>();
                
                SetDayOfTheMonthPreferenceOn(customerPreferences);
                SetDayOfTheWeekPreferenceOn(customerPreferences);
                SetMasterPreferenceOn(customerPreferences);

                if (customerPreferences.Count > 0)
                {
                    Customers.Add(new Customer(customerName, customerPreferences));
                }
            }
        }

        private void SetDayOfTheMonthPreferenceOn(ICollection<IPreference> customerPreferences)
        {
            var daysOfTheMonth = new List<int>();
            var dayOfTheMonthChoices = _lineElements[1].Split(',');
            
            foreach (var choice in dayOfTheMonthChoices)
            {
                var isPositiveInteger = ushort.TryParse(choice, out var dayOfTheMonth);
                if (isPositiveInteger && dayOfTheMonth is >= 1 and <= 28)
                {
                    daysOfTheMonth.Add(dayOfTheMonth);
                }
            }

            if (daysOfTheMonth.Count == 0)
                return;
            
            customerPreferences.Add(new DayOfTheMonthPreference(daysOfTheMonth.ToArray()));
        }

        private void SetDayOfTheWeekPreferenceOn(ICollection<IPreference> customerPreferences)
        {
            var daysOfTheWeekChoices = _lineElements[2].Split(',');
            var weekDayPreference = DaysOfTheWeek.None;
            
            foreach (var choice in daysOfTheWeekChoices)
            {
                var isDaysOfTheWeek = Enum.TryParse(choice, true, out DaysOfTheWeek dayOfTheWeek); 
                if (isDaysOfTheWeek)
                {
                    weekDayPreference |= dayOfTheWeek;
                }
            }

            if (weekDayPreference == DaysOfTheWeek.None)
                return;

            customerPreferences.Add(new DayOfTheWeekPreference(weekDayPreference));
        }

        private void SetMasterPreferenceOn(ICollection<IPreference> customerPreferences)
        {
            var hasMasterPreference = bool.TryParse(_lineElements[3], out var masterPreferenceChoice);
            
            if (!hasMasterPreference)
                return;
            
            IPreference masterPreference = masterPreferenceChoice ? new EveryDayPreference() : new DontContactPreference();
            customerPreferences.Add(masterPreference);
        }
    }
}