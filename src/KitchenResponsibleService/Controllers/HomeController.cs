using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
		readonly KitchenService kitchenService;

		public HomeController(KitchenService kitchenService)
		{
			this.kitchenService = kitchenService;
		}

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var weeksAndResponsibles = await kitchenService.GetWeeksAndResponsibles();
            ViewData["WeeksAndResponsibles"] = weeksAndResponsibles;
            return View();
        }
    }
}
