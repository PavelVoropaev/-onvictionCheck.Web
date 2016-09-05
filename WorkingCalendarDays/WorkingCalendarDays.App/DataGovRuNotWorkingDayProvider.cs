using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using WorkingCalendarDays.Core.Entity;
using WorkingCalendarDays.Core.Interface;

namespace WorkingCalendarDays.App
{
    internal class DataGovRuNotWorkingDayProvider : INotWorkingDayProvider
    {
        private readonly IDictionary<int, List<CalendarDay>> _holidaysByYearDictionary;

        public DataGovRuNotWorkingDayProvider(string pathToFile)
        {
            _holidaysByYearDictionary = new Dictionary<int, List<CalendarDay>>();
            FillCache(pathToFile);
        }

        public List<CalendarDay> GetYearNotWorkingCalendarDays(int year)
        {
            return _holidaysByYearDictionary[year];
        }

        private void FillCache(string pathToFile)
        {
            using (var textReader = File.OpenText(pathToFile))
            {
                var csv = new CsvReader(textReader);
                while (csv.Read())
                {
                    var year = csv.GetField<int>(0);
                    var holidays = new List<CalendarDay>();

                    for (int i = 1; i <= 12; i++)
                    {
                        var holidayDaysInMounth = csv.GetField<string>(i).Split("*,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                        holidays.AddRange(
                            holidayDaysInMounth.Select(holiday => new CalendarDay { Day = new DateTime(year, i, holiday) }));
                    }

                    _holidaysByYearDictionary.Add(year, holidays);
                }
            }
        }
    }
}
