# Solar Enegry Feed Importer

Imports lifetime energy data and daily statistics from public Enphase solar data feeds.

Change the SystemId to match the desired system (i.e. 1173801 for Solar Roadways)

You can find the system Id from the end of the Url on the "Take me to the logged-in" view button.


## Build/Run

DotNet Core 2.0 application. Needs VS2017 or VS Code to run.

## Creates

Creates 3 csv files that can be opened with Microsoft Excel

* DailyEnergyByDay.csv
* LifetimeEnergyByDay.csv
* LifetimeEnergyMonthly.csv

### DailyEnergyByDay

Row per day with energy (Watt-hours) per interval (typically 15 minute) per column.

Note: Some columns (esp. night time) have null/empty value rather than 0's. Not sure why. Is their a threshold on the sensors below which power or voltage it ignores the data or assumes the panels are disconnected???

### LifetimeEnergyByDay

Day sequential data, date and total Watt-hours columns.

### LifetimeEnergyMonthly

Data is split into rows for each month, with total Watt-hours per day in columns.