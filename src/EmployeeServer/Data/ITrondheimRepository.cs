using KitchenResponsible.Model;

namespace KitchenResponsible.Data {
    public interface ITrondheimRepository {
        ResponsibleForWeek GetResponsibleForThisWeekAndNext(ushort week, ushort nextWeek);
    }
}