using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IEmployeeService
    {
        ResponsibleForWeek GetEmployeeForWeek();
        IEnumerable<Employee> Get();
        Employee Get(int id);
    }
}