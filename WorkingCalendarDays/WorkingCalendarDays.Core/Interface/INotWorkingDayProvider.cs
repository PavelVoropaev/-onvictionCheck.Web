using System.Collections.Generic;
using WorkingCalendarDays.Core.Entity;

namespace WorkingCalendarDays.Core.Interface
{
    public interface INotWorkingDayProvider
    {
        List<CalendarDay> GetYearNotWorkingCalendarDays(int year);
    }
}