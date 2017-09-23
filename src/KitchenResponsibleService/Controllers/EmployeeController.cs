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
		readonly KitchenService kitchenService;

		public EmployeeController(KitchenService kitchenService)
		{
            this.kitchenService = kitchenService;
		}

        // POST api/kitchen
        [HttpPost]
        public Task Post([FromBody]string employeeId) =>
            kitchenService.AddNewEmployee(employeeId);
    }
}
