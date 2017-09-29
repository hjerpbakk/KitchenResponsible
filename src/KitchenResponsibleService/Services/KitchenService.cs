using System;
using System.Linq;
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

        public async Task AddNewEmployee(string employeeId, string fullName) {
            if (employeeId == null) {
                throw new ArgumentNullException(nameof(employeeId));
            }

			if (fullName == null)
			{
				throw new ArgumentNullException(nameof(fullName));
			}

            await blobStorage.AddNewEmployee(employeeId, fullName);
            await RemoveOldWeeksAndFillWithFreeEmployees();
		}

        public async Task<IEnumerable<ResponsibleForWeek>> GetWeeksAndResponsibles(bool detailed = false) =>
            await RemoveOldWeeksAndFillWithFreeEmployees(detailed); 

        public async Task<ResponsibleForWeek> GetWeekAndResponsibleForEmployee(string employeeId) {
			if (employeeId == null)
			{
				throw new ArgumentNullException(nameof(employeeId));
			}

            var weeksWithResponisbles = await RemoveOldWeeksAndFillWithFreeEmployees();
            var weekForUser = weeksWithResponisbles.SingleOrDefault(w => w.SlackUser == employeeId);
            if (weekForUser.SlackUser == null) {
                return new ResponsibleForWeek(0, employeeId);
            }

            return weekForUser;
		}

        public async Task<ResponsibleForWeek> GetWeekAndResponsibleForWeek(ushort weekNumber) {
            if (weekNumber <= 0 || weekNumber > 52) {
                throw new ArgumentOutOfRangeException(nameof(weekNumber));
            }

			var weeksWithResponisbles = await RemoveOldWeeksAndFillWithFreeEmployees();
            var weekForUser = weeksWithResponisbles.SingleOrDefault(w => w.WeekNumber == weekNumber);
			if (weekForUser.SlackUser == null)
			{
				return new ResponsibleForWeek(weekNumber, null);
			}

			return weekForUser;
        }

        public async Task<ResponsibleForWeek> GetWeekAndResponsibleForCurrentWeek() =>
            await GetWeekAndResponsibleForWeek(GetIso8601WeekOfYear(ConfigurableDateTime.UtcNow));

        public async Task RemoveEmployee(string employeeId) {
            // TODO: Fjern bruker fra listen og etterfyll ukene dersom det blir hull
            await blobStorage.RemoveEmployee(employeeId);
        }
            
        async Task<List<ResponsibleForWeek>> RemoveOldWeeksAndFillWithFreeEmployees(bool detailed = false) {
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
            ushort week;
            var lastWeekWithResponsible = weeksAndResponsibles.LastOrDefault();
            if (lastWeekWithResponsible.WeekNumber == 0) {
                week = GetIso8601WeekOfYear(ConfigurableDateTime.UtcNow);    
            } else {
                week = lastWeekWithResponsible.WeekNumber;
            }

            foreach (var employee in employees)
            {
                if (weeksAndResponsibles.FindIndex(w => w.SlackUser == employee) == -1) {
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
