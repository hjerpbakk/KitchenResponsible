using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace KitchenResponsible.Data {
    public class TeamMembersRepo {
        public IList<string> Test() {
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
    }
}
