using System.Collections.Generic;
using System.IO;
using System.Threading;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services {
    public class EmployeeService : IEmployeeService {
        const char SplitChar = ';';

        // TODO: Crap for now
        public IEnumerable<Employee> Get() {
            Thread.Sleep(500);
            return GetEmployees();
        }

        public Employee Get(int id) {
            Thread.Sleep(500);
            return GetEmployees()[id];
        }

        private static List<Employee> GetEmployees() {
            var employeesPath = Path.Combine("" + "Employees.txt");
            var lines = File.ReadAllLines(employeesPath);
            var employeesFromDisk = new List<Employee>(lines.Length);
            for (int i = 0; i < lines.Length; i++) {
                var splittedLine = lines[i].Split(SplitChar);
                var weekResponsible = ushort.Parse(splittedLine[0]);
                var name = splittedLine[1].Trim();
                employeesFromDisk.Add(new Employee(i, weekResponsible, name, "y0l0", name));
            }

            return employeesFromDisk;
        }
    }
}