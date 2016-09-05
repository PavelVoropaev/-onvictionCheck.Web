using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using WorkingCalendarDays.Core.Entity;
using WorkingCalendarDays.Core.Interface;
using WorkingCalendarDays.Core.Logic;

namespace WorkingCalendarDays.Tests
{
    [TestFixture]
    public class WorkingDayManagerTests
    {
        private MockRepository _mocks;
        private INotWorkingDayProvider _notWorkingDayProvider;
        private IWorkingDayManager _workingDayManager;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
            _notWorkingDayProvider = _mocks.DynamicMock<INotWorkingDayProvider>();
            _workingDayManager = new WorkingDayManager(_notWorkingDayProvider);
        }

        [Test]
        public void GetNextDay_NoHolidaysInPeriod()
        {
            const int dayMove = 5;
            const int notWorkingDayCount = 0;
            const int assertTotalDays = dayMove;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 05, 05));
        }

        [Test]
        public void GetNextDay_AllHolidaysInPeriod()
        {
            const int dayMove = 5;
            const int notWorkingDayCount = 5;
            const int assertTotalDays = dayMove + notWorkingDayCount;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 05, 05));
        }

        [Test]
        public void GetNextDay_DoubleHolidaysPeriod()
        {
            var dayMove = 5;
            var notWorkingDayCount = 10;
            var assertTotalDays = dayMove + notWorkingDayCount;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 05, 05));
        }


        [Test]
        public void GetNextDay_HalfHolidaysPeriod()
        {
            var dayMove = 6;
            var notWorkingDayCount = 3;
            var assertTotalDays = dayMove + notWorkingDayCount;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 05, 05));
        }

        [Test]
        public void GetNextDay_NewYearHolidaysPeriod()
        {
            var dayMove = 6;
            var notWorkingDayCount = 3;
            var assertTotalDays = dayMove + notWorkingDayCount;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 12, 29));
        }


        [Test]
        public void GetNextDay_LongPeriod()
        {
            var dayMove = 1000;
            var notWorkingDayCount = 30;
            var assertTotalDays = dayMove + notWorkingDayCount;
            TestGetNextDay(notWorkingDayCount, dayMove, assertTotalDays, new DateTime(2016, 10, 29));
        }

        private void TestGetNextDay(int notWorkingDayCount, int dayMove, int assertTotalDays, DateTime startDateTime)
        {
            var notWirkingDays = new List<CalendarDay>();
            for (int i = 0; i < notWorkingDayCount; i++)
            {
                notWirkingDays.Add(new CalendarDay { Day = startDateTime.AddDays(i) });
            }

            foreach (var holidaysGroupedByYear in notWirkingDays.GroupBy(x => x.Day.Year))
            {
                _notWorkingDayProvider.Stub(m => m.GetYearNotWorkingCalendarDays(holidaysGroupedByYear.Key)).Return(holidaysGroupedByYear.ToList());
            }

            _notWorkingDayProvider.Stub(m => m.GetYearNotWorkingCalendarDays(2)).IgnoreArguments().Return(new List<CalendarDay>());

            _mocks.ReplayAll();
            var nextDay = _workingDayManager.GetNextDay(startDateTime, dayMove);
            Assert.AreEqual(nextDay.Date, startDateTime.Date.AddDays(assertTotalDays));
        }
    }
}