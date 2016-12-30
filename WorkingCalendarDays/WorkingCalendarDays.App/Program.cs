using System;
using System.Configuration;
using System.Globalization;
using WorkingCalendarDays.Core.Logic;

namespace WorkingCalendarDays.App
{
    class Program
    {
        static void Main(string[] args)
        {
            int dayMove;
            DateTime day;
            TimeSpan cacheExpirationTime;
            string pathToFile;

            if (!TryParceArgs(args, out dayMove, out day, out cacheExpirationTime, out pathToFile))
            {
                return;
            }

            var notWorkingDayCacheProvider = new NotWorkingDayCacheProvider(new DataGovRuNotWorkingDayProvider(pathToFile), cacheExpirationTime);
            var workingDayManager = new WorkingDayManager(notWorkingDayCacheProvider);
            var nextDay = workingDayManager.GetNextDay(day.Date, dayMove);

            Console.WriteLine($"NextDay: {nextDay.Date}");
        }

        private static bool TryParceArgs(
            string[] args,
            out int dayMove,
            out DateTime day,
            out TimeSpan cacheExpirationTime,
            out string pathToFile)
        {
            dayMove = 0;
            day = DateTime.MinValue;
            cacheExpirationTime = TimeSpan.MinValue;
            pathToFile = string.Empty;

            if (args.Length != 2)
            {
                return false;
            }

            var dayStr = args[0];
            var dayMoveStr = args[1];
            if (
                !int.TryParse(dayMoveStr, out dayMove)
                || !DateTime.TryParseExact(dayStr, "dd.mm.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out day))
            {
                Console.WriteLine($"Invalid input args: dayStr {dayStr}, dayMove {dayMoveStr}");
                return false;
            }

            var cacheExpirationTimeStr = ConfigurationManager.AppSettings["cacheExpirationTime"];
            if (!TimeSpan.TryParse(cacheExpirationTimeStr, out cacheExpirationTime))
            {
                Console.WriteLine($"Invalid appConfig cacheExpirationTime: {cacheExpirationTimeStr}");
                return false;
            }

            pathToFile = ConfigurationManager.AppSettings["pathToFileWithNotWorkingDays"];
            Console.WriteLine($"Input args startDay: {day.Date}, dayMove: {dayMove}, cacheExpirationTime: {cacheExpirationTime}, pathToFileWithNotWorkingDays: {pathToFile}");
            return true;
        }
    }
}
