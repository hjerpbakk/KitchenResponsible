using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using KitchenResponsibleService.Db;
using KitchenResponsibleService.Model;

namespace KitchenResponsibleService.Services
{
    public class KitchenService
    {
        readonly IStorage blobStorage;

        public KitchenService(IStorage blobStorage)
        {
            this.blobStorage = blobStorage;
        }

        public async Task AddNewEmployee(string employeeId) {
            await blobStorage.AddNewEmployee(employeeId);
            await RemoveOldWeeksAndFillWithFreeEmployees();
		}

        public async Task<IEnumerable<ResponsibleForWeek>> GetWeeksAndResponsibles() =>
            await RemoveOldWeeksAndFillWithFreeEmployees(); 

        async Task<List<ResponsibleForWeek>> RemoveOldWeeksAndFillWithFreeEmployees() {
			var weeksWithResponisbles = await blobStorage.GetWeeksAndResponsibles();
            RemoveOldWeeks(weeksWithResponisbles);
            await GiveWeeksToFreeEmployees(weeksWithResponisbles);
			await blobStorage.Save(weeksWithResponisbles);
            return weeksWithResponisbles;
        }

        void RemoveOldWeeks(List<ResponsibleForWeek> weeksAndResponsibles) {
            var thisWeek = GetIso8601WeekOfYear(ConfigurableDateTime.UtcNow);
            var i = weeksAndResponsibles.FindIndex(w => w.WeekNumber == thisWeek);
            if (i > 0) {
                weeksAndResponsibles.RemoveRange(0, i);    
            }
        }

        async Task GiveWeeksToFreeEmployees(List<ResponsibleForWeek> weeksAndResponsibles) {
            var employees = await blobStorage.GetEmployees();
            var week = GetIso8601WeekOfYear(ConfigurableDateTime.UtcNow);
            foreach (var employee in employees)
            {
                if (weeksAndResponsibles.FindIndex(w => w.SlackUserId == employee) == -1) {
                    week = GetNextWeek(week);
                    weeksAndResponsibles.Add(new ResponsibleForWeek(week, employee));
                }
            }
        }

		static ushort GetIso8601WeekOfYear(DateTime time)
		{
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

		static ushort GetNextWeek(ushort week) =>
			week == 52 ? (ushort)1 : (ushort)(week + 1);

		static ushort GetPreviousWeek(ushort week) =>
			week == 1 ? (ushort)52 : (ushort)(week - 1);
    }
}
