using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
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

        public ResponsibleForWeek GetEmployeeForWeek() {
            // TODO: Transaksjon over hele her
            var week = weekNumberFinder.GetIso8601WeekOfYear(DateTime.UtcNow);
            var weeksWithResponsible = repository.GetWeeksWithResponsible();
            
            var weeksToDelete = FindPastWeeks(week, weeksWithResponsible);
            if (weeksToDelete.Count > 0) {
                RemovePastWeeksAndAddNewOnces(weeksToDelete, weeksWithResponsible);
            }

            // TODO: Benytt heller dataene som allerede er hentet               
            return repository.GetResponsibleForThisWeekAndNext(week, WeekNumberFinder.GetNextWeek(week));
        }

        private IReadOnlyList<Week> FindPastWeeks(ushort week, IReadOnlyList<Week> weeksWithResponsible) {
            var weeksToDelete = new List<Week>();
            var previousWeek = WeekNumberFinder.GetPreviousWeek(week);
            Week weekWithResponsible;
            while((weekWithResponsible = weeksWithResponsible.SingleOrDefault(w => w.WeekNumber == previousWeek)).Responsible != null) {
                weeksToDelete.Add(weekWithResponsible);
                previousWeek = WeekNumberFinder.GetPreviousWeek(previousWeek);
            }

            return weeksToDelete;
        }

        private void RemovePastWeeksAndAddNewOnces(IReadOnlyList<Week> weeksToDelete, IReadOnlyList<Week> weeksWithResponsible) {
            repository.DeleteWeeks(weeksToDelete.Select(w => w.WeekNumber).ToArray());
                
                // De som ble fjernet må fordeles på framtidige uker
                var newResponsiblesForWeeks = new Week[weeksToDelete.Count];
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

                for (int i = 0; i < weeksToDelete.Count; i++) {
                    lastWeek = WeekNumberFinder.GetNextWeek(lastWeek);
                    newResponsiblesForWeeks[i] = new Week(lastWeek, weeksToDelete[i].Responsible, 0);
                }
                
                repository.InsertWeeks(newResponsiblesForWeeks);
        }
    }
}