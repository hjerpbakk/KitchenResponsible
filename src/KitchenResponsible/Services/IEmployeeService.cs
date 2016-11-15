using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IEmployeeService
    {
        ResponsibleForWeek GetEmployeeForWeek();
        IEnumerable<string> Get();
    }
}