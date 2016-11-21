using System;
using System.Globalization;

namespace KitchenResponsible.Utils.DateAndTime {
    public class WeekNumberFinder : IWeekNumberFinder {
        /// <summary>
        /// This presumes that weeks start with Monday. 
        /// Week 1 is the 1st week of the year with a Thursday in it.
        /// </summary>
        public ushort GetIso8601WeekOfYear(DateTime time) {
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

        public ushort GetNextWeek(ushort week) => week == 52 ? (ushort)1 : (ushort)(week + 1);
    }
}

         