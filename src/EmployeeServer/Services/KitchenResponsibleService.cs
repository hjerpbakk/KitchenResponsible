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
        readonly ITrondheimRepository trondheimRepository;
        readonly IWeekNumberFinder weekNumberFinder;

        public KitchenResponsibleService(ITrondheimRepository trondheimRepository, IWeekNumberFinder weekNumberFinder) {
            this.trondheimRepository = trondheimRepository;  
            this.weekNumberFinder = weekNumberFinder;   
        }

        // TODO: Update when new week and satisfy old tests...
        public ResponsibleForWeek GetEmployeeForWeek() {
            var week = weekNumberFinder.GetIso8601WeekOfYear(DateTime.UtcNow);
            var nextWeek = WeekNumberFinder.GetNextWeek(week);
            return trondheimRepository.GetResponsibleForThisWeekAndNext(week, nextWeek);
        }
    }
}