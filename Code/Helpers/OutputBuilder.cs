using System;
using System.Collections.Generic;

namespace Code.Helpers
{
    public class OutputBuilder
    {
        private readonly IList<Customer> _customers;
        private readonly DateTimeOffset _fromDate;
        private readonly int _numberOfDays;

        public OutputBuilder(DateTimeOffset fromDate, IList<Customer> customers, int numberOfDays)
        {
            _fromDate = fromDate;
            _customers = customers;
            _numberOfDays = numberOfDays;
        }

        public IList<string> Build()
        {
            var output = new List<string>();
            
            for (var daysToAdd = 0; daysToAdd <= _numberOfDays; daysToAdd++)
            {
                var sendDate = _fromDate.AddDays(daysToAdd);
                var customersToContact = new List<string>();
                
                foreach (var customer in _customers)
                {
                    if (customer.WantsPreferencesOn(sendDate))
                    {
                        customersToContact.Add(customer.Name);
                    }
                }

                var printDate = sendDate.ToString("D");
                var hasCustomersToContact = customersToContact.Count > 0;

                output.Add(hasCustomersToContact
                    ? $"{printDate}: {string.Join(',', customersToContact)}"
                    : $"{printDate}: There are no customers to contact on this day");
            }

            return output;
        }
    }
}