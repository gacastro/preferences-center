# Customer Preferences Centre
Customers are able to set their preferences for receiving marketing information. The following options are available:
* On a specified date of the month [1-28]
* On each specified day of the week [MON-SUN] (collection)
* Every day
* Never

<br/>

- [Customer Preferences Centre](#customer-preferences-centre)
  - [The Idea](#the-idea)
  - [Considerations](#considerations)
  - [How to run](#how-to-run)
  - [Performance](#performance)
  - [Future improvements](#future-improvements)

<br/>

## The Idea

Implement a system that accepts the choices of multiple customers as input.
After receiving the input the system should produce a report of the upcoming 90 days.
For each day that marketing material will be sent, the report should show which customers will be a recipient

For example,
* Customer A chooses 'Every day'.
* Customer B chooses 'On the 10th of the month'.
* Customer C chooses ‘On Tuesday and Friday’.
After providing this input the abridged output beginning in April would be:

```
Sun 01-April-2018 A
Mon 02-April-2018 A
Tue 03-April-2018 A,C
Wed 04-April-2018 A
Thu 05-April-2018 A
Fri 06-April-2018 A,C
Sat 07-April-2018 A
Sun 08-April-2018 A
Mon 09-April-2018 A
Tue 10-April-2018 A,B,C
Wed 11-April-2018 A
Thu 12-April-2018 A
Fri 13-April-2018 A,C
Sat 14-April-2018 A
...
```

## Considerations
* Application is going to be developed as a console application in .Net
* The choices of the customers will be passed in via a file
* The input file is limited to 100KB in order to be safely loaded into memory
* There are only unique entries in the file.
* Each customer preference is a line in the file. And the format is `<CustomerName>|[<DaysOfTheMonth>]|[<DaysOfTheWeek>]|<AllDays>`
  * For example,
    * Customer A chooses 'Every day' => `CustomerA|-|-|true`
    * Customer B chooses 'On the 10th of the month' => `CustomerB|10|-|-`
    * Customer C chooses ‘On Tuesday and Friday’ => `CustomerC|-|Tuesday,Friday|-`
  * When All days is true it means everyday. And when its false it means do not contact
* Customer can pick all three types of preferences, days of the month, of the week, and all days. But All days takes precedence over the other ones

## How to run
.Net core needs to be installed in your machine.

From your command line navigate to the root folder of the application and:
```
> cd Code
> dotnet run --f '1sample-customer-preferences.file'
```

Considering the output is larger than your average command line, you can run the above command as shown bellow to push it into a file
```
dotnet run --f '1sample-customer-preferences.file' > output.txt
```

## Performance
For the worst case scenario:
* time complexity is O(90n) which in reality is O(n)
* space complexity is O(n)

## Future improvements
* At the moment, there are some assumptions on how the input will be. Going forwars we should eliminate all assumptions.
* The file size as been restricted to 100KB. But going forward we should change the way we read the file and perform a load test. The load test would allow us to understand which size of file the application can take.
