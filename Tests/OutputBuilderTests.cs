using System;
using System.Collections.Generic;
using Code;
using Code.Helpers;
using Code.Preferences;
using Xunit;

namespace Tests
{
    public class OutputBuilderTests
    {
        private readonly List<Customer> _customers;
        private readonly DateTimeOffset _dateToTest;

        public OutputBuilderTests()
        {
            _customers = new List<Customer>
            {
                new("joe", new List<IPreference>
                {
                    new DontContactPreference()
                }),
                new("jack", new List<IPreference>
                {
                    new EveryDayPreference()
                }),
                new("jim", new List<IPreference>
                {
                    new DayOfTheWeekPreference(DaysOfTheWeek.Monday | DaysOfTheWeek.Wednesday | DaysOfTheWeek.Sunday)
                }),
                new("bob", new List<IPreference>
                {
                    new DayOfTheMonthPreference(new[]{1,5})
                })
            };
            
            _dateToTest = new DateTimeOffset(2021, 5, 3, 0, 0, 0, TimeSpan.Zero);
        }

        [Fact]
        public void no_customers_found()
        {
            var customers = new List<Customer>
            {
                new("doe", new List<IPreference>
                {
                    new DayOfTheMonthPreference(new[] {4})
                }),
                new("rod", new List<IPreference>
                {
                    new DayOfTheWeekPreference(DaysOfTheWeek.Thursday)
                })
            };
            
            var output =
                new OutputBuilder(
                        _dateToTest,
                        customers,
                        2)
                    .Build();
            
            Assert.Equal(3, output.Count);
            Assert.Equal("Monday, 03 May 2021: There are no customers to contact on this day", output[0]);
            Assert.Equal("Tuesday, 04 May 2021: doe", output[1]);
            Assert.Equal("Wednesday, 05 May 2021: There are no customers to contact on this day", output[2]);
        }
        
        [Fact]
        public void can_build_an_output()
        {
            var output =
                new OutputBuilder(
                        _dateToTest,
                        _customers,
                        2)
                    .Build();
            
            Assert.Equal(3, output.Count);
            Assert.Equal("Monday, 03 May 2021: jack,jim", output[0]);
            Assert.Equal("Tuesday, 04 May 2021: jack", output[1]);
            Assert.Equal("Wednesday, 05 May 2021: jack,jim,bob", output[2]);
        }

        [Fact]
        public void can_build_from_past_dates()
        {
            var output =
                new OutputBuilder(
                        _dateToTest.AddDays(-3),
                        _customers,
                        5)
                    .Build();
            
            Assert.Equal(6, output.Count);
            Assert.Equal("Friday, 30 April 2021: jack", output[0]);
            Assert.Equal("Saturday, 01 May 2021: jack,bob", output[1]);
            Assert.Equal("Sunday, 02 May 2021: jack,jim", output[2]);
            Assert.Equal("Monday, 03 May 2021: jack,jim", output[3]);
            Assert.Equal("Tuesday, 04 May 2021: jack", output[4]);
            Assert.Equal("Wednesday, 05 May 2021: jack,jim,bob", output[5]);
        }
    }
}