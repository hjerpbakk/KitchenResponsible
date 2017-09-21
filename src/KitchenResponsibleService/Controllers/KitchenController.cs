using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenResponsibleService.Model;
using KitchenResponsibleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class KitchenController : Controller
    {
        readonly KitchenService kitcenService;

        public KitchenController(KitchenService kitcenService)
        {
            this.kitcenService = kitcenService;
        }

        // GET api/kitchen
        [HttpGet]
		public async Task<IEnumerable<ResponsibleForWeek>> Get()
		{
            await kitcenService.AddNewEmployee("");
			return new ResponsibleForWeek[] {
                new ResponsibleForWeek(41, "U1TBU8336"),
                new ResponsibleForWeek(42, "U1TBU8336")};
		}

    }
}
