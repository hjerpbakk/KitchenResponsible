using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Acquaint.Models;
using Newtonsoft.Json;

namespace Acquaint.Data {
	public class TeamMemberService {

		readonly HttpClient httpClient;

		public TeamMemberService() {
			httpClient = new HttpClient();
		}

		public async Task<TeamMemberLite[]> Get() {
			try {
				var response = await httpClient.GetStringAsync("http://localhost:5000/api/employees");
				var teamMembers = JsonConvert.DeserializeObject<TeamMemberLite[]>(response);
				return teamMembers;
			} catch (Exception e) {
				return new TeamMemberLite[0];
			}
		}

		// TODO: Client side cache
		public async Task<TeamMember> Get(TeamMemberLite teamMemberLite) {
			try {
				var response = await httpClient.GetStringAsync($"http://localhost:5000/api/employees/{teamMemberLite.Id}");
				var teamMember = JsonConvert.DeserializeObject<TeamMember>(response);
				teamMember.TeamMemberLite = teamMemberLite;
				return teamMember;
			} catch (Exception e) {
				return new TeamMember("", "");
			}
		}
	}
}
