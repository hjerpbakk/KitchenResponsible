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

		public async Task<IEnumerable<TeamMemberLite>> Get() {
			try {
				var response = await httpClient.GetStringAsync("http://localhost:5000/api/employees");
				var teamMembers = JsonConvert.DeserializeObject<IEnumerable<TeamMemberLite>>(response);
				return teamMembers;
			} catch (Exception e) {
				return new List<TeamMemberLite>();
			}
		}

		public async Task<TeamMember> Get(int id) {
			try {
				var response = await httpClient.GetStringAsync($"http://localhost:5000/api/employees/{id}");
				var teamMember = JsonConvert.DeserializeObject<TeamMember>(response);
				return teamMember;
			} catch (Exception e) {
				return new TeamMember(-1, "", "", "", "", "", "");
			}
		}
	}
}
