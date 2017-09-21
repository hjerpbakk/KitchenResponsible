using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
		readonly KitchenService kitcenService;

		public EmployeeController(KitchenService kitcenService)
		{
			this.kitcenService = kitcenService;
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}
    }
}
