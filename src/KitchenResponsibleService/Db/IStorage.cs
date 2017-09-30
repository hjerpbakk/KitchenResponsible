using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenResponsibleService.Model;

namespace KitchenResponsibleService.Db
{
    public interface IStorage
    {
        Task AddNewEmployee(Employee employee);
        Task RemoveEmployee(string employeeId);
        Task<Employee[]> GetEmployees();
        Task<List<ResponsibleForWeek>> GetWeeksAndResponsibles();
        Task Save(List<ResponsibleForWeek> weeksAndResponsibles);
    }
}
