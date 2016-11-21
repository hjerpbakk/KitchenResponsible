using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IKitchenResponsibleService
    {
        ResponsibleForWeek GetEmployeeForWeek();
    }
}