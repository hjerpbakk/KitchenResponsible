using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Data {
    public interface ITrondheimRepository {
        ResponsibleForWeek GetResponsibleForThisWeekAndNext(ushort week, ushort nextWeek);
        IReadOnlyList<Week> GetWeeksWithResponsible();
        void DeleteWeeks(IEnumerable<ushort> weeks);
        void InsertWeeks(Week[] weeks);
    }
}