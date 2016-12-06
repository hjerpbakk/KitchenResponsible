using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using KitchenResponsible.Data;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services {
    public class EmployeeService : IEmployeeService {
        readonly ITrondheimRepository repository;

        public EmployeeService(ITrondheimRepository trondheimRepository) {
            this.repository = trondheimRepository;  
        }

        public Employee[] Get() {
            Thread.Sleep(500);
            return repository.GetNicks().Select(e => new Employee(0, 1, "a", "b", e)).ToArray();
        }

        public Employee Get(int id) {
            Thread.Sleep(500);
            return new Employee(0, 1, "a", "b");
        }
    }
}