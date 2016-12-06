using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Acquaint.Models;
using Newtonsoft.Json;

namespace Acquaint.Data
{
	public class KitchenResponsibleService
	{
		readonly HttpClient httpClient;

		public KitchenResponsibleService()
		{
			httpClient = new HttpClient();
		}

		public async Task<Week[]> Get()
		{
			try
			{
				var response = await httpClient.GetStringAsync("http://localhost:5000/api/kitchenresponsible");
				var teamMembers = JsonConvert.DeserializeObject<Week[]>(response);
				return teamMembers;
			}
			catch (Exception e)
			{
				return new Week[0];
			}
		}
	}
}
