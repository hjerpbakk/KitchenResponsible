using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Services;
using KitchenResponsibleService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace KitchenResponsibleService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
		readonly KitchenService kitchenService;
        readonly IMemoryCache memoryCache;

		public HomeController(KitchenService kitchenService, IMemoryCache memoryCache)
		{
			this.kitchenService = kitchenService;
            this.memoryCache = memoryCache;
		}

        public async Task<IActionResult> Index()
        {
            IEnumerable<ResponsibleForWeek> weeksAndResponsibles;
            // Look for cache key.
            if (!memoryCache.TryGetValue(Keys.WeeksAndResponsibles, out weeksAndResponsibles))
            {
                // Key not in cache, so get data.
                weeksAndResponsibles = await kitchenService.GetWeeksAndResponsibles();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(59));

                // Save data in cache.
                memoryCache.Set(Keys.WeeksAndResponsibles, weeksAndResponsibles, cacheEntryOptions);
            }

            ViewData["WeeksAndResponsibles"] = weeksAndResponsibles;
            return View();
        }
    }
}
