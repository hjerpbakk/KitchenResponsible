using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public IReadOnlyList<Week> GetWeeksWithResponsible() {
            using (var connection = OpenConnection()) {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Week, Responsible FROM KitchenResponsible ORDER BY Week";
                var weeksWithResponsible = new List<Week>();
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        weeksWithResponsible.Add(new Week(ushort.Parse(reader.GetString(0)), reader.GetString(1)));
                    }
                }

                return weeksWithResponsible;
             }
        }

        public void DeleteWeeks(ushort[] weeks) {
            if (weeks == null) {
                throw new ArgumentNullException(nameof(weeks));
            }

            if (weeks.Length == 0) {
                return;
            }

            using (var connection = OpenConnection()) {
                var command = connection.CreateCommand();
                var weeksSQL = String.Join(",", weeks.Select(w => w.ToString()));
                command.CommandText = $"DELETE FROM KitchenResponsible WHERE Week IN ({weeksSQL})";
                command.ExecuteScalar();
            }
        }

        public void InsertWeeks(Week[] weeks) {
            if (weeks == null) {
                throw new ArgumentNullException(nameof(weeks));
            }

            if (weeks.Length == 0) {
                return;
            }

            using (var connection = OpenConnection()) {
                var command = connection.CreateCommand();
                var weeksSQL = String.Join(",", weeks.Select(w => $"({w.WeekNumber}, \"{w.Responsible}\")"));
                command.CommandText = $"INSERT INTO KitchenResponsible (Week, Responsible) VALUES {weeksSQL}";
                command.ExecuteScalar();
            }
        }

        public void AddNewEmployee(Employee employee) {
            if (employee == null) {
                throw new ArgumentNullException(nameof(employee));
            }

            // TODO: Må få kjøkkenskift
            using (var connection = OpenConnection()) {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO TeamMembers (Nick) VALUES (\"$nick\")";
                command.Parameters.AddWithValue("$nick", employee.Nick);
                command.ExecuteNonQuery();
            }
        }

        // TODO: Remove employee
        // TODO: Må fjerne skiftet på kjøkkenet og lagt inn en av de andre

        private SqliteConnection OpenConnection() {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
