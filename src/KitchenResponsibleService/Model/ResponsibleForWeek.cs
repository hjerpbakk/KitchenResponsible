using System;
using System.Linq;
using System.Collections.Generic;

namespace KitchenResponsibleService.Model
{
    public struct ResponsibleForWeek : IEquatable<ResponsibleForWeek>
    {
        public ResponsibleForWeek(ushort weekNumber, string slackUserId)
        {
            WeekNumber = weekNumber;
            SlackUserId = slackUserId;
        }

        public ushort WeekNumber { get; }

        public string SlackUserId { get; }

        public static List<ResponsibleForWeek> Parse(string weeksAndResponsibles) {
            return weeksAndResponsibles.Split('\n').
                                       Select(line => line.Split(' ')).
                                       Select(split => new ResponsibleForWeek(ushort.Parse(split[0]), split[1])).
                                       ToList();
        }

        public static string Parse(IEnumerable<ResponsibleForWeek> weeksAndResponsibles) {
            return string.Join("\n", weeksAndResponsibles.Select(w => $"{w.WeekNumber} {w.SlackUserId}"));
        }

        public bool Equals(ResponsibleForWeek other) =>
            WeekNumber == other.WeekNumber && SlackUserId == other.SlackUserId;
    }
}
