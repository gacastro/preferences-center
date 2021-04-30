using System;
using System.Collections.Generic;
using Code;
using Code.Helpers;
using Code.Preferences;
using Xunit;

namespace Tests
{
    public class IdentifyPreferencesTests
    {
        private const string John = "john";

        [Fact]
        public void returns_true_when_customer_has_selected_everyday()
        {
            var customerPreferences = new List<IPreference>
            {
                new EveryDayPreference()
            };
            var customer = new Customer(John, customerPreferences);

            var result = customer.WantsPreferencesOn(DateTimeOffset.UtcNow);
            
            Assert.True(result);
        }

        [Fact]
        public void returns_false_when_customer_has_selected_do_not_contact()
        {
            var customerPreferences = new List<IPreference>
            {
                new DontContactPreference()
            };
            var customer = new Customer(John, customerPreferences);

            var result = customer.WantsPreferencesOn(DateTimeOffset.UtcNow);
            
            Assert.False(result);
        }

        [Theory]
        [InlineData(30, true)]
        [InlineData(29, true)]
        [InlineData(28, false)]
        public void assert_when_customer_chose_a_day_in_the_week(int day, bool expected)
        {
            var chosenDays = DaysOfTheWeek.Friday | DaysOfTheWeek.Thursday;
            var customerPreferences = new List<IPreference>
            {
                new DayOfTheWeekPreference(chosenDays)
            };
            var customer = new Customer(John, customerPreferences);

            var result = customer.WantsPreferencesOn(new DateTimeOffset(2021,4,day,0,0,0,TimeSpan.Zero));
            
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(1, false)]
        [InlineData(23, true)]
        public void assert_when_customer_chose_a_date_in_the_month(int day, bool expected)
        {
            var chosenDays = new[] {2, 23};
            var customer = new Customer(John, new List<IPreference>
            {
                new DayOfTheMonthPreference(chosenDays)
            });
            
            var result = customer.WantsPreferencesOn(new DateTimeOffset(2021,4,day,0,0,0,TimeSpan.Zero));
            
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(1, false)]
        [InlineData(16, true)]
        [InlineData(23, true)]
        public void assert_when_customer_chose_a_day_of_the_week_and_date_of_the_month(int day, bool expected)
        {
            var chosenMonthDays = new[] {2, 23};
            var chosenWeekDays = DaysOfTheWeek.Tuesday | DaysOfTheWeek.Wednesday | DaysOfTheWeek.Friday;
            var customer = new Customer(John, new List<IPreference>
            {
                new DayOfTheMonthPreference(chosenMonthDays),
                new DayOfTheWeekPreference(chosenWeekDays)
            });
            
            var result = customer.WantsPreferencesOn(new DateTimeOffset(2021,4,day,0,0,0,TimeSpan.Zero));
            
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(6)]
        [InlineData(27)]
        [InlineData(30)]
        public void everyday_preference_takes_precedence_over_day_specific_preferences(int day)
        {
            var chosenMonthDays = new[] {4, 21};
            var chosenWeekDays = DaysOfTheWeek.Monday;
            var customer = new Customer(John, new List<IPreference>
            {
                new EveryDayPreference(),
                new DayOfTheMonthPreference(chosenMonthDays),
                new DayOfTheWeekPreference(chosenWeekDays)
            });

            var result = customer.WantsPreferencesOn(new DateTimeOffset(2021, 4, day, 0, 0, 0, TimeSpan.Zero));
            
            Assert.True(result);
        }
        
        [Theory]
        [InlineData(4)]
        [InlineData(21)]
        [InlineData(12)]
        public void do_not_contact_preference_takes_precedence_over_day_specific_preferences(int day)
        {
            var chosenMonthDays = new[] {4, 21};
            var chosenWeekDays = DaysOfTheWeek.Monday;
            var customer = new Customer(John, new List<IPreference>
            {
                new DontContactPreference(),
                new DayOfTheMonthPreference(chosenMonthDays),
                new DayOfTheWeekPreference(chosenWeekDays)
            });

            var result = customer.WantsPreferencesOn(new DateTimeOffset(2021, 4, day, 0, 0, 0, TimeSpan.Zero));
            
            Assert.False(result);
        }
    }
}