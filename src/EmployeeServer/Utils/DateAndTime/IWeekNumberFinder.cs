using System;

namespace KitchenResponsible.Utils.DateAndTime {
    public interface IWeekNumberFinder {
        /// <summary>
        /// This presumes that weeks start with Monday. 
        /// Week 1 is the 1st week of the year with a Thursday in it.
        /// </summary>
        ushort GetIso8601WeekOfYear(DateTime time);
    }
}