using System;
using System.Linq;
using WorkingCalendarDays.Core.Interface;

namespace WorkingCalendarDays.Core.Logic
{
    public class WorkingDayManager : IWorkingDayManager
    {
        private readonly INotWorkingDayProvider _notWorkingDayProvider;

        public WorkingDayManager(INotWorkingDayProvider notWorkingDayProvider)
        {
            _notWorkingDayProvider = notWorkingDayProvider;
        }

        public DateTime GetNextDay(DateTime startDay, int dayMove)
        {
            var countNotWorkingDays = GetCountNotWorkingDays(startDay, dayMove);
            return startDay.AddDays(countNotWorkingDays + dayMove);
        }

        private int GetCountNotWorkingDays(DateTime startDate, int dayMove, int calculatedNotWorkingDays = 0)
        {
            var endCheckPeriodDate = startDate + TimeSpan.FromDays(dayMove);

            var holidays = _notWorkingDayProvider.GetYearNotWorkingCalendarDays(startDate.Year);
            if (startDate.Year != endCheckPeriodDate.Year)
            {
                holidays.AddRange(_notWorkingDayProvider.GetYearNotWorkingCalendarDays(endCheckPeriodDate.Year));
            }

            var holidaysInPeriod = holidays.Where(x => x.Day >= startDate && x.Day < startDate.AddDays(dayMove)).ToList();
            var holidaysCountInPeriod = holidaysInPeriod.Count;

            var totalNotWorkingDays = holidaysCountInPeriod + calculatedNotWorkingDays;

            if (holidaysCountInPeriod != 0)
            {
                return GetCountNotWorkingDays(endCheckPeriodDate, holidaysCountInPeriod, totalNotWorkingDays);
            }

            return totalNotWorkingDays;
        }
    }
}
