using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using WorkingCalendarDays.Core.Entity;
using WorkingCalendarDays.Core.Interface;

namespace WorkingCalendarDays.Core.Logic
{
    public class NotWorkingDayCacheProvider : INotWorkingDayProvider
    {
        private readonly ObjectCache _notWorkingDayByYearCache;

        private readonly INotWorkingDayProvider _notWorkingDayProvider;

        private readonly TimeSpan _cacheExpirationTimeSpan;

        public NotWorkingDayCacheProvider(INotWorkingDayProvider notWorkingDayProvider, TimeSpan cacheExpirationTime)
        {
            _notWorkingDayByYearCache = new MemoryCache("NotWorkingDayCache");
            _cacheExpirationTimeSpan = cacheExpirationTime;
            _notWorkingDayProvider = notWorkingDayProvider;
        }

        public List<CalendarDay> GetYearNotWorkingCalendarDays(int year)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheExpirationTimeSpan)
            };

            List<CalendarDay> holidays;
            if (!_notWorkingDayByYearCache.Contains(year.ToString()))
            {
                holidays = _notWorkingDayProvider.GetYearNotWorkingCalendarDays(year);
                _notWorkingDayByYearCache.Set(year.ToString(), holidays, policy);
            }
            else
            {
                holidays = _notWorkingDayByYearCache[year.ToString()] as List<CalendarDay>;
            }

            return holidays;
        }
    }
}
