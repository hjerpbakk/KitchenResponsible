using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IKitchenResponsibleService
    {
        ResponsibleForWeek GetEmployeeForWeek();
        Week[] GetWeeksWithResponsible();
    }
}