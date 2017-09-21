using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenResponsibleService.Model;

namespace KitchenResponsibleService.Db
{
    public interface IStorage
    {
        Task AddNewEmployee(string employeeId);
        Task<string[]> GetEmployees();
        Task<List<ResponsibleForWeek>> GetWeeksAndResponsibles();
        Task Save(List<ResponsibleForWeek> weeksAndResponsibles);
    }
}
