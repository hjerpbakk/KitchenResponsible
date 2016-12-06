using System.Collections.Generic;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public interface IEmployeeService
    {
        Employee[] Get();
        Employee Get(int id);
    }
}