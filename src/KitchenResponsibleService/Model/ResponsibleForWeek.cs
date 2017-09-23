using System;
using System.Linq;
using System.Collections.Generic;

namespace KitchenResponsibleService.Model
{
    public struct ResponsibleForWeek : IEquatable<ResponsibleForWeek>
    {
        public ResponsibleForWeek(ushort weekNumber, string slackUser)
        {
            WeekNumber = weekNumber;
            SlackUser = slackUser;
        }

        public ushort WeekNumber { get; }

        public string SlackUser { get; }

        public static List<ResponsibleForWeek> Parse(string weeksAndResponsibles) {
            return weeksAndResponsibles.TrimEnd('\n').Split('\n').
                                       Select(line => line.Split(' ')).
                                       Select(split => new ResponsibleForWeek(ushort.Parse(split[0]), split[1])).
                                       ToList();
        }

        public static string Parse(IEnumerable<ResponsibleForWeek> weeksAndResponsibles) {
            return string.Join("\n", weeksAndResponsibles.Select(w => $"{w.WeekNumber} {w.SlackUser}"));
        }

        public bool Equals(ResponsibleForWeek other) =>
            WeekNumber == other.WeekNumber && SlackUser == other.SlackUser;
    }
}
