using System;
using System.Linq;
using System.Collections.Generic;
using KitchenResponsible.Model;
using KitchenResponsible.Data;
using KitchenResponsible.Utils.DateAndTime;

namespace KitchenResponsible.Services {
    public class KitchenResponsibleService : IKitchenResponsibleService {
        readonly ITrondheimRepository repository;
        readonly IWeekNumberFinder weekNumberFinder;

        public KitchenResponsibleService(ITrondheimRepository trondheimRepository, IWeekNumberFinder weekNumberFinder) {
            this.repository = trondheimRepository;  
            this.weekNumberFinder = weekNumberFinder;   
        }

        // TODO: More array, less readonly list
        public Week[] GetWeeksWithResponsible() {
            // TODO: Hva med oppdatering ang uke, ref. GetEmployeeForWeek
            // TODO: CACHE
            var weeksWithResponsible = repository.GetWeeksWithResponsible();
            var lastWeekOfYear = WeekNumberFinder.GetLastWeekOfYear(DateTime.UtcNow.Year);
            var lastWeek = GetLastWeek(weeksWithResponsible, lastWeekOfYear);
            var newYear = new List<Week>();
            var oldYear = new List<Week>();
            for (int i = 0; i < weeksWithResponsible.Count; i++) {
                if (weeksWithResponsible[i].WeekNumber <= lastWeek) {
                    newYear.Add(weeksWithResponsible[i]);
                } else {
                    oldYear.Add(weeksWithResponsible[i]);
                }
            }

            oldYear.AddRange(newYear);
            return oldYear.ToArray();
        }

        public ResponsibleForWeek GetEmployeeForWeek() {
            var week = weekNumberFinder.GetIso8601WeekOfYear();
            var weeksWithResponsible = repository.GetWeeksWithResponsible();
            var allTeamMembers = repository.GetNicks();
            var lastWeekOfYear = WeekNumberFinder.GetLastWeekOfYear(DateTime.UtcNow.Year);

            var weeksToDelete = RemovePastWeeks(week, weeksWithResponsible, lastWeekOfYear);
            if (weeksToDelete.Count > 0) {
                AddNewWeeks(weeksToDelete, weeksWithResponsible, allTeamMembers, lastWeekOfYear);
            }

            var responsible = weeksWithResponsible.Single(w => w.WeekNumber == week).Responsible;
            var upNext = weeksWithResponsible.Single(w => w.WeekNumber == WeekNumberFinder.GetNextWeek(week, lastWeekOfYear)).Responsible;
            return new ResponsibleForWeek(week, responsible, upNext);
        }

        private IReadOnlyList<Week> RemovePastWeeks(ushort week, IReadOnlyList<Week> weeksWithResponsible, ushort lastWeekOfYear) {
            var weeksToDelete = new List<Week>();
            var previousWeek = WeekNumberFinder.GetPreviousWeek(week, lastWeekOfYear);
            Week weekWithResponsible;
            while((weekWithResponsible = weeksWithResponsible.SingleOrDefault(w => w.WeekNumber == previousWeek)).Responsible != null) {
                weeksToDelete.Add(weekWithResponsible);
                previousWeek = WeekNumberFinder.GetPreviousWeek(previousWeek, lastWeekOfYear);
            }

            return weeksToDelete;
        }

        private void AddNewWeeks(IReadOnlyList<Week> weeksToDelete, IReadOnlyList<Week> weeksWithResponsible, IReadOnlyList<string> allTeamMembers, ushort lastWeekOfYear) {              
            var lastWeek = GetLastWeek(weeksWithResponsible, lastWeekOfYear);
            var teamMembersWithoutDuty = allTeamMembers.Where(t => !weeksWithResponsible.Any(w => w.Responsible == t)).ToArray();
            var newResponsiblesForWeeks = new Week[weeksToDelete.Count + teamMembersWithoutDuty.Length];
            var j = 0;
            for (; j < teamMembersWithoutDuty.Length; j++) {
                lastWeek = WeekNumberFinder.GetNextWeek(lastWeek, lastWeekOfYear);
                newResponsiblesForWeeks[j] = new Week(lastWeek, teamMembersWithoutDuty[j]);
            }
            
            for (; j < newResponsiblesForWeeks.Length; j++) {
                lastWeek = WeekNumberFinder.GetNextWeek(lastWeek, lastWeekOfYear);
                newResponsiblesForWeeks[j] = new Week(lastWeek, weeksToDelete[j - teamMembersWithoutDuty.Length].Responsible);
            }

            repository.RemovePastWeeksAndAddNewOnces(weeksToDelete.Select(w => w.WeekNumber).ToArray(), newResponsiblesForWeeks);
        }

        private ushort GetLastWeek(IReadOnlyList<Week> weeksWithResponsible, ushort lastWeekOfYear) {
            int prev = 0;
            if (weeksWithResponsible.Count > 50) {
                throw new Exception("Too many employees!");
            }
            
            if (weeksWithResponsible.Last().WeekNumber == lastWeekOfYear) {
                for (int i = 0; i < lastWeekOfYear; ++i) {
                    if (weeksWithResponsible[i].WeekNumber - prev > 1) {
                        return weeksWithResponsible[i - 1].WeekNumber;
                    } else {
                        prev++;
                    }
                }
            } 

            return weeksWithResponsible.Last().WeekNumber;
        }
    }
}