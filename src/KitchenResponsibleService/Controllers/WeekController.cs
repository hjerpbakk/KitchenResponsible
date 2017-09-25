using System.Threading.Tasks;
using KitchenResponsibleService.Model;
using KitchenResponsibleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class WeekController : Controller
    {
		readonly KitchenService kitchenService;

		public WeekController(KitchenService kitchenService)
		{
			this.kitchenService = kitchenService;
		}

		// GET api/week
		[HttpGet]
		public async Task<ResponsibleForWeek> Get() =>
            await kitchenService.GetWeekAndResponsibleForCurrentWeek();


		// GET api/week/5
		[HttpGet("{weekNumber}")]
		public async Task<ResponsibleForWeek> Get(int weekNumber) =>
            await kitchenService.GetWeekAndResponsibleForWeek((ushort)weekNumber);
    }
}
