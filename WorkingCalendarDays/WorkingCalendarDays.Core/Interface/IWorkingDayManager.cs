using System;

namespace WorkingCalendarDays.Core.Interface
{
    public interface IWorkingDayManager
    {
        DateTime GetNextDay(DateTime startDay, int dayMove);
    }
}