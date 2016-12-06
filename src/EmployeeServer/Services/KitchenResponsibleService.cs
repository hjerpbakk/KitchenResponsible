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

        public IReadOnlyList<Week> GetWeeksWithResponsible() {
            // TODO: Hva med oppdatering ang uke????
            // TODO: CACHE
            return repository.GetWeeksWithResponsible();
        }

        public ResponsibleForWeek GetEmployeeForWeek() {
            var week = weekNumberFinder.GetIso8601WeekOfYear();
            var weeksWithResponsible = repository.GetWeeksWithResponsible();
            var allTeamMembers = repository.GetNicks();
            
            var weeksToDelete = RemovePastWeeks(week, weeksWithResponsible);
            if (weeksToDelete.Count > 0) {
                AddNewWeeks(weeksToDelete, weeksWithResponsible, allTeamMembers);
            }

            var responsible = weeksWithResponsible.Single(w => w.WeekNumber == week).Responsible;
            var upNext = weeksWithResponsible.Single(w => w.WeekNumber == WeekNumberFinder.GetNextWeek(week)).Responsible;
            return new ResponsibleForWeek(week, responsible, upNext);
        }

        private IReadOnlyList<Week> RemovePastWeeks(ushort week, IReadOnlyList<Week> weeksWithResponsible) {
            var weeksToDelete = new List<Week>();
            var previousWeek = WeekNumberFinder.GetPreviousWeek(week);
            Week weekWithResponsible;
            while((weekWithResponsible = weeksWithResponsible.SingleOrDefault(w => w.WeekNumber == previousWeek)).Responsible != null) {
                weeksToDelete.Add(weekWithResponsible);
                previousWeek = WeekNumberFinder.GetPreviousWeek(previousWeek);
            }

            return weeksToDelete;
        }

        private void AddNewWeeks(IReadOnlyList<Week> weeksToDelete, IReadOnlyList<Week> weeksWithResponsible, IReadOnlyList<string> allTeamMembers) {              
            ushort lastWeek = 0;
            int prev = 0;
            if (weeksWithResponsible.Last().WeekNumber == 52) {
                for (int i = 0; i < 52; ++i) {
                    if (weeksWithResponsible[i].WeekNumber - prev > 1) {
                        lastWeek = weeksWithResponsible[i - 1].WeekNumber;
                        break;
                    } else {
                        prev++;
                    }
                }
            } else {
                lastWeek = weeksWithResponsible.Last().WeekNumber;
            }

            var teamMembersWithoutDuty = allTeamMembers.Where(t => !weeksWithResponsible.Any(w => w.Responsible == t)).ToArray();
            var newResponsiblesForWeeks = new Week[weeksToDelete.Count + teamMembersWithoutDuty.Length];
            var j = 0;
            for (; j < teamMembersWithoutDuty.Length; j++) {
                lastWeek = WeekNumberFinder.GetNextWeek(lastWeek);
                newResponsiblesForWeeks[j] = new Week(lastWeek, teamMembersWithoutDuty[j]);
            }
            
            for (; j < newResponsiblesForWeeks.Length; j++) {
                lastWeek = WeekNumberFinder.GetNextWeek(lastWeek);
                newResponsiblesForWeeks[j] = new Week(lastWeek, weeksToDelete[j - teamMembersWithoutDuty.Length].Responsible);
            }

            repository.RemovePastWeeksAndAddNewOnces(weeksToDelete.Select(w => w.WeekNumber).ToArray(), newResponsiblesForWeeks);
        }
    }
}