using System;
using Code.Helpers;
using Code.Preferences;
using Xunit;

namespace Tests
{
    public class CustomerParserTests
    {
        [Fact]
        public void can_parse_customer_name()
        {
            var lines = new[] {"CustomerA|1,15,23|-|-"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
        }
        
        [Fact]
        public void can_parse_days_of_the_month_preference()
        {
            var lines = new[] {"CustomerA|1,15,23|-|-"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.Single(customer.DaySpecificPreferences);
            var dayOfTheMonthPreference = customer.DaySpecificPreferences[0];
            Assert.IsType<DayOfTheMonthPreference>(dayOfTheMonthPreference);
            Assert.True(dayOfTheMonthPreference.IsToSendOn(new DateTimeOffset(2021,4,1,0,0,0, TimeSpan.Zero)));
            Assert.True(dayOfTheMonthPreference.IsToSendOn(new DateTimeOffset(2021,4,15,0,0,0, TimeSpan.Zero)));
            Assert.True(dayOfTheMonthPreference.IsToSendOn(new DateTimeOffset(2021,4,23,0,0,0, TimeSpan.Zero)));
            Assert.False(dayOfTheMonthPreference.IsToSendOn(new DateTimeOffset(2021,4,5,0,0,0, TimeSpan.Zero)));
        }

        [Fact]
        public void can_parse_days_of_the_week_preference()
        {
            var lines = new[] {"CustomerA|-|wednesday,friday|-"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.Single(customer.DaySpecificPreferences);
            var dayOfTheWeekPreference = customer.DaySpecificPreferences[0];
            Assert.IsType<DayOfTheWeekPreference>(dayOfTheWeekPreference);
            Assert.True(dayOfTheWeekPreference.IsToSendOn(new DateTimeOffset(2021,4,28,0,0,0, TimeSpan.Zero)));
            Assert.True(dayOfTheWeekPreference.IsToSendOn(new DateTimeOffset(2021,4,30,0,0,0, TimeSpan.Zero)));
            Assert.False(dayOfTheWeekPreference.IsToSendOn(new DateTimeOffset(2021,4,29,0,0,0, TimeSpan.Zero)));
        }

        [Theory]
        [InlineData("true", typeof(EveryDayPreference))]
        [InlineData("false", typeof(DontContactPreference))]
        public void can_parse_master_preference(string option, Type preferenceType)
        {
            var lines = new[] {$"CustomerA|-|-|{option}"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.IsType(preferenceType, customer.MasterPreference);
        }

        [Fact]
        public void can_ignore_customer()
        {
            var lines = new[] {"CustomerA|-|-|-"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Empty(customers);
        }

        [Theory]
        [InlineData("-")]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("29")]
        public void can_ignore_day_of_the_month_preference(string wrongDay)
        {
            var lines = new[] {$"CustomerA|{wrongDay}|wednesday|false"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.DoesNotContain(
                customer.DaySpecificPreferences,
                preference => preference.GetType() == typeof(DayOfTheMonthPreference));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("-1")]
        [InlineData("wedneday")]
        public void can_ignore_day_of_the_week_preference(string wrongWeekDay)
        {
            var lines = new[] {$"CustomerA|1,3,27|{wrongWeekDay}|false"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.DoesNotContain(
                customer.DaySpecificPreferences,
            preference => preference.GetType() == typeof(DayOfTheWeekPreference));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("not bool")]
        [InlineData("trueeee")]
        public void can_ignore_master_preference(string wrongMasterPreference)
        {
            var lines = new[] {$"CustomerA|1,3,27|wednesday,friday|{wrongMasterPreference}"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.Null(customer.MasterPreference);
        }

        [Fact]
        public void can_parse_all_preferences()
        {
            var lines = new[] {"CustomerA|1,3,15|wednesday|true"};
            var customers = new CustomerParser(lines).Customers;
            
            Assert.Single(customers);
            var customer = customers[0];
            Assert.Equal("customera", customer.Name);
            Assert.IsType<EveryDayPreference>(customer.MasterPreference);
            foreach (var preference in customer.DaySpecificPreferences)
            {
                if (preference is DayOfTheWeekPreference)
                {
                    Assert.True(preference.IsToSendOn(new DateTimeOffset(2021,4,14,0,0,0, TimeSpan.Zero)));
                    Assert.False(preference.IsToSendOn(new DateTimeOffset(2021,4,15,0,0,0, TimeSpan.Zero)));
                }
                else
                {
                    Assert.True(preference.IsToSendOn(new DateTimeOffset(2021,4,1,0,0,0,TimeSpan.Zero)));
                    Assert.True(preference.IsToSendOn(new DateTimeOffset(2021,4,3,0,0,0,TimeSpan.Zero)));
                    Assert.True(preference.IsToSendOn(new DateTimeOffset(2021,4,15,0,0,0,TimeSpan.Zero)));
                    Assert.False(preference.IsToSendOn(new DateTimeOffset(2021,4,18,0,0,0,TimeSpan.Zero)));
                }
            }
        }
    }
}