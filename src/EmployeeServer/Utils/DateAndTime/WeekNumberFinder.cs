using System;
using System.Globalization;

namespace KitchenResponsible.Utils.DateAndTime {
    public class WeekNumberFinder : IWeekNumberFinder {
        /// <summary>
        /// This presumes that weeks start with Monday. 
        /// Week 1 is the 1st week of the year with a Thursday in it.
        /// </summary>
        public ushort GetIso8601WeekOfYear() => 
            GetIso8601WeekOfYear(DateTime.UtcNow);

        public static ushort GetNextWeek(ushort week, ushort lastWeek) => 
            week == lastWeek ? (ushort)1 : (ushort)(week + 1);
        public static ushort GetPreviousWeek(ushort week, ushort lastWeek) => 
            week == 1 ? (ushort)lastWeek : (ushort)(week - 1);

        private static ushort GetIso8601WeekOfYear(DateTime time) {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return (ushort)CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static ushort GetLastWeekOfYear(int year) => 
            GetIso8601WeekOfYear(new DateTime(year, 12, 31));
    }
}

         