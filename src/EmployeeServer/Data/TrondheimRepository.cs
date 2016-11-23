using System.Collections.Generic;
using KitchenResponsible.Model;
using Microsoft.Data.Sqlite;

namespace KitchenResponsible.Data {
    public class TrondheimRepository : ITrondheimRepository {
        readonly string connectionString;

        // TODO: async all these calls?
        public TrondheimRepository() {
            connectionString = "Filename=" + "Trondheim.db";
        }

        public IList<string> GetNicks() {
            using (var connection = new SqliteConnection("Filename=" + "Trondheim.db")) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Nick FROM TeamMembers";
                var nicks = new List<string>();
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        nicks.Add(reader.GetString(0));
                    }
                }

                return nicks;
            }
        }

        public ResponsibleForWeek GetResponsibleForThisWeekAndNext(ushort week, ushort nextWeek) {
            using (var connection = OpenConnection()) {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Responsible FROM KitchenResponsible WHERE Week = $week or Week = $nextWeek ORDER BY Week";
                command.Parameters.AddWithValue("$week", week);
                command.Parameters.AddWithValue("$nextWeek", nextWeek);
                var responsibles = new string[2];
                using (var reader = command.ExecuteReader()) {
                    var i = 0;
                    while (reader.Read()) {
                        responsibles[i++] = reader.GetString(0);
                    }
                }

                return nextWeek == 1 ? new ResponsibleForWeek(week, responsibles[1], responsibles[0]) : new ResponsibleForWeek(week, responsibles[0], responsibles[1]);
            }
        }

        private SqliteConnection OpenConnection() {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
