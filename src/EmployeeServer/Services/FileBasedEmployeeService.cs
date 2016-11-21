using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using KitchenResponsible.Model;

namespace KitchenResponsible.Services
{
    public class FileBasedEmployeeService : IKitchenResponsibleService
    {
        const char SplitChar = ';';
        static string employeesPath;
        static string currentWeekPath;

        static readonly Random random;

        private static ResponsibleForWeek? currentWeek;
        private static List<Employee> employees;             

        static FileBasedEmployeeService() {
            random = new Random();
        }       

        public FileBasedEmployeeService(IOptions<Paths> paths) {
            if (employeesPath == null) {
                employeesPath = Path.Combine(paths.Value.FilePath + "Employees.txt");
                employees = GetEmployees();
            }

            if (currentWeekPath == null) {
                currentWeekPath = Path.Combine(paths.Value.FilePath + "CurrentWeek.txt");
            }
            
            if (!currentWeek.HasValue) {
                currentWeek = GetCurrentWeek();
            }
        }
       
        public ResponsibleForWeek GetEmployeeForWeek()
        {           
            var week = Week.GetIso8601WeekOfYear(DateTime.UtcNow);
            if (currentWeek.Value.Week == week) {
                return currentWeek.Value;
            }

            employees = GetEmployees();
            var candidates = employees.Where(emp => emp.WeekResponsible == currentWeek.Value.Week || emp.WeekResponsible == 0).
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
            SetCurrentWeek(responsible.Nick, onDeck.Nick);
            return currentWeek.Value;
        }

        private static ResponsibleForWeek GetCurrentWeek()
        {
            var lines = File.ReadAllLines(currentWeekPath);
            var weekNumber = ushort.Parse(lines[0]);
            var name = lines[1];
            var onDeck = lines[2];
            return new ResponsibleForWeek(weekNumber, name, onDeck);
        }

        private static List<Employee> GetEmployees()
        {
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
            var newWeekNumber = (ushort)(currentWeek.Value.Week + 1);
            currentWeek = new ResponsibleForWeek(newWeekNumber, responsible, onDeck);
            var lines = new string[3];
            lines[0] = currentWeek.Value.Week.ToString();
            lines[1] = currentWeek.Value.Responsible;
            lines[2] = currentWeek.Value.OnDeck;
            File.WriteAllLines(currentWeekPath, lines);
        }

        private static void WriteEmployees()
        {
            var lines = new string[employees.Count];
            for (var i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                lines[i] = $"{employee.WeekResponsible}{SplitChar}{employee.Nick}";
            }

            File.WriteAllLines(employeesPath, lines);
        }
    }
}