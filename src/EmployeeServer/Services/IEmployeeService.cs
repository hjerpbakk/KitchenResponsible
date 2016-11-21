using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> Get();
        Employee Get(int id);
    }
}