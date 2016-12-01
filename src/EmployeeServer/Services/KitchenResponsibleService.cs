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

        // Insert
        // Sett inn i neste ledige uke, få like mange weeks som den som har mest

        public ResponsibleForWeek GetEmployeeForWeek() {
            // TODO: Transaksjon over hele her
            var week = weekNumberFinder.GetIso8601WeekOfYear(DateTime.UtcNow);
            var weeksWithResponsible = repository.GetWeeksWithResponsible();
            
            // Alle uker før denne må slettes
            var weeksToDelete = new List<Week>();
            var previousWeek = WeekNumberFinder.GetPreviousWeek(week);
            Week weekWithResponsible;
            while((weekWithResponsible = weeksWithResponsible.SingleOrDefault(w => w.WeekNumber == previousWeek)).Responsible != null) {
                weeksToDelete.Add(weekWithResponsible);
                previousWeek = WeekNumberFinder.GetPreviousWeek(previousWeek);
            }
                        
            repository.DeleteWeeks(weeksToDelete.Select(w => w.WeekNumber));
            
            // De som ble fjernet må fordeles på framtidige uker
            var newResponsiblesForWeeks = new Week[weeksToDelete.Count];
            ushort lastWeek = 1;
            if (weeksWithResponsible.Last().WeekNumber == 52) {
                while (weeksWithResponsible.SingleOrDefault(w => w.WeekNumber == lastWeek++).Responsible != null) {
                }
            } else {
                lastWeek = weeksWithResponsible.Last().WeekNumber;
            }

            for (int i = 0; i < weeksToDelete.Count; i++) {
                lastWeek = WeekNumberFinder.GetNextWeek(lastWeek);
                newResponsiblesForWeeks[i] = new Week(lastWeek, weeksToDelete[i].Responsible, 0);
            }

            repository.InsertWeeks(newResponsiblesForWeeks);

            // TODO: Benytt heller dataene som allerede er hentet               
            return repository.GetResponsibleForThisWeekAndNext(week, WeekNumberFinder.GetNextWeek(week));
        }
    }
}