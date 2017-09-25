using System.Threading.Tasks;
using KitchenResponsibleService.Model;
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

        // POST api/employee
        [HttpPost]
        public async Task Post([FromBody]string employeeId) =>
            await kitchenService.AddNewEmployee(employeeId);

		// GET api/employee/5
		[HttpGet("{employeeId}")]
		public async Task<ResponsibleForWeek> Get(string employeeId) =>
			await kitchenService.GetWeekAndResponsibleForEmployee(employeeId);
    }
}
