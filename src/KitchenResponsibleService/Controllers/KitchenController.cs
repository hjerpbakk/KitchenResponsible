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
        readonly KitchenService kitchenService;

        public KitchenController(KitchenService kitchenService)
        {
            this.kitchenService = kitchenService;
        }

        // GET api/kitchen
        [HttpGet]
        public async Task<IEnumerable<ResponsibleForWeek>> Get() =>
            await kitchenService.GetWeeksAndResponsibles();
    }
}
