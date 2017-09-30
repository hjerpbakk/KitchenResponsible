using System;
using System.Linq;
using System.Collections.Generic;

namespace KitchenResponsibleService.Model
{
    public struct ResponsibleForWeek : IEquatable<ResponsibleForWeek>
    {
        const char SplitCharacter = ';';

        public ResponsibleForWeek(ushort weekNumber, Employee employee)
        {
            WeekNumber = weekNumber;
            SlackUser = employee.SlackUserId;
            Name = employee.Name;
        }

        public ushort WeekNumber { get; }
        public string SlackUser { get; }
        public string Name { get; }

        public static List<ResponsibleForWeek> Parse(string weeksAndResponsibles) {
            return weeksAndResponsibles.TrimEnd('\n').Split('\n').
                                       Select(line => line.Split(SplitCharacter)).
                                       Select(split => new ResponsibleForWeek(ushort.Parse(split[0]), new Employee(split[1], split[2]))).
                                       ToList();
        }

        public static string Parse(IEnumerable<ResponsibleForWeek> weeksAndResponsibles) {
            return string.Join("\n", weeksAndResponsibles.Select(w => $"{w.WeekNumber}{SplitCharacter}{w.SlackUser}{SplitCharacter}{w.Name}"));
        }

        public bool Equals(ResponsibleForWeek other) =>
            WeekNumber == other.WeekNumber && SlackUser == other.SlackUser && Name == other.Name;

        public override string ToString() =>
            string.Format("[ResponsibleForWeek: WeekNumber={0}, SlackUser={1}], Name={2}]", WeekNumber, SlackUser, Name);
        
    }
}
