using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Acquaint.Models;
using Newtonsoft.Json;

namespace Acquaint.Data {
	public class EmployeeService {

		readonly HttpClient httpClient;

		public EmployeeService() {
			httpClient = new HttpClient();
		}

		public async Task<IEnumerable<Acquaintance>> GetItems() {
			try {
				var response = await httpClient.GetStringAsync("http://localhost:5000/api/employees");
				var employees = JsonConvert.DeserializeObject<IEnumerable<Acquaintance>>(response);
				return employees;
			} catch (Exception e) {
				return new List<Acquaintance>();
			}
		}
	}
}
