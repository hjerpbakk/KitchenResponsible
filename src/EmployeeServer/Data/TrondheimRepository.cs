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

        public IReadOnlyList<string> GetNicks() {
            Thread.Sleep(500);
            using (var connection = OpenConnection()) {
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
            Thread.Sleep(500);
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

        public void RemovePastWeeksAndAddNewOnces(ushort[] passedWeeks, Week[] newWeeks) {
            Thread.Sleep(500);
            if (passedWeeks == null) {
                throw new ArgumentNullException(nameof(passedWeeks));
            }

            if (newWeeks == null) {
                throw new ArgumentNullException(nameof(newWeeks));
            }          

            using (var connection = OpenConnection()) {
                var transaction = connection.BeginTransaction();
                if (passedWeeks.Length == 0) {
                    return;
                }
                var removeCommand = connection.CreateCommand();
                var remove = String.Join(",", passedWeeks.Select(w => w.ToString()));
                removeCommand.CommandText = $"DELETE FROM KitchenResponsible WHERE Week IN ({remove})";
                removeCommand.ExecuteScalar();

                if (newWeeks.Length == 0) {
                    return;
                }

                var insertCommand = connection.CreateCommand();
                var insert = String.Join(",", newWeeks.Select(w => $"({w.WeekNumber}, \"{w.Responsible}\")"));
                insertCommand.CommandText = $"INSERT INTO KitchenResponsible (Week, Responsible) VALUES {insert}";
                insertCommand.ExecuteScalar();
                transaction.Commit();
            }          
        }

        public void AddNewEmployee(Employee employee) {
            Thread.Sleep(500);
            if (employee == null) {
                throw new ArgumentNullException(nameof(employee));
            }

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
