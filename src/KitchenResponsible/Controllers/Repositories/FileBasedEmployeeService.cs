using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace KitchenResponsible.Controllers.Repositories
{
    public class FileBasedEmployeeService : IEmployeeService
    {
        const char SplitChar = ';';
        const string Path = "C:\\Users\\roh.DIPS-AD\\Dropbox\\KPI\\";
        const string EmployeesPath = Path + "Employees.txt";
        const string CurrentWeekPath = Path + "CurrentWeek.txt";

        static readonly Random random;

        private static ResponsibleForWeek currentWeek;
        private static List<Employee> employees;

        static FileBasedEmployeeService()
        {
            random = new Random();
            currentWeek = GetCurrentWeek();
            employees = GetEmployees();
        }

        public ResponsibleForWeek GetEmployeeForWeek(ushort week)
        {
            if (currentWeek.Week == week)
            {
                return currentWeek;
            }

            employees = GetEmployees();
            var candidates = employees.Where(emp => emp.WeekResponsible == currentWeek.Week || emp.WeekResponsible == 0).
                OrderBy(emp => random.NextDouble()).
                ToArray();
            var highestWeek = GetHighestWeek();
            var nextWeeks = GetNextWeeks(highestWeek, candidates.Length);
            for (var i = 0; i < candidates.Length; i++)
            {
                candidates[i].WeekResponsible = nextWeeks[i];
            }

            var responsible = employees.Single(emp => emp.WeekResponsible == week);
            var onDeck = employees.Single(emp => emp.WeekResponsible == GetNextWeek(week));

            WriteEmployees();
            SetCurrentWeek(responsible.Name, onDeck.Name);
            return currentWeek;
        }

        private static ResponsibleForWeek GetCurrentWeek()
        {
            var lines = File.ReadAllLines(CurrentWeekPath);
            var weekNumber = ushort.Parse(lines[0]);
            var name = lines[1];
            var onDeck = lines[2];
            return new ResponsibleForWeek(weekNumber, name, onDeck);
        }

        private static List<Employee> GetEmployees()
        {
            var lines = File.ReadAllLines(EmployeesPath);
            var employeesFromDisk = new List<Employee>(lines.Length);
            foreach (var line in lines)
            {
                var splittedLine = line.Split(SplitChar);
                var weekResponsible = ushort.Parse(splittedLine[0]);
                var name = splittedLine[1].Trim();
                employeesFromDisk.Add(new Employee(weekResponsible, name));
            }

            return employeesFromDisk;
        }

        private static ushort GetHighestWeek()
        {
            var orderedEmployees = employees.OrderBy(emp => emp.WeekResponsible).ToArray();
            if (orderedEmployees.Last().WeekResponsible != 52)
            {
                return orderedEmployees.Last().WeekResponsible;
            }

            for (var i = 1; i < orderedEmployees.Length; i++)
            {
                if (orderedEmployees[i].WeekResponsible - orderedEmployees[i - 1].WeekResponsible > orderedEmployees.Length)
                {
                    return orderedEmployees[i - 1].WeekResponsible;
                }
            }
                
            throw new Exception("Too many employees");
        }

        private static ushort[] GetNextWeeks(ushort week, int n)
        {
            var nextWeeks = new ushort[n];
            var nextWeek = GetNextWeek(week);
            for (var i = 0; i < n; i++)
            {
                nextWeeks[i] = nextWeek;
                nextWeek = GetNextWeek(nextWeek);
            }

            return nextWeeks;
        }

        private static ushort GetNextWeek(ushort week) => week == 52 ? (ushort)1 : (ushort)(week + 1);

        private static void SetCurrentWeek(string responsible, string onDeck)
        {
            var newWeekNumber = (ushort)(currentWeek.Week + 1);
            currentWeek = new ResponsibleForWeek(newWeekNumber, responsible, onDeck);
            var lines = new string[3];
            lines[0] = currentWeek.Week.ToString();
            lines[1] = currentWeek.Responsible;
            lines[2] = currentWeek.OnDeck;
            File.WriteAllLines(CurrentWeekPath, lines);
        }

        private static void WriteEmployees()
        {
            var lines = new string[employees.Count];
            for (var i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                lines[i] = $"{employee.WeekResponsible}{SplitChar}{employee.Name}";
            }

            File.WriteAllLines(EmployeesPath, lines);
        }
    }
}