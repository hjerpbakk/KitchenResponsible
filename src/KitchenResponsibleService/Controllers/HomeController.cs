using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Services;
using KitchenResponsibleService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using KitchenResponsibleService.Clients;

namespace KitchenResponsibleService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
		readonly KitchenService kitchenService;
        readonly IMemoryCache memoryCache;
        readonly ComicsClient comicsClient;

		public HomeController(KitchenService kitchenService, IMemoryCache memoryCache, ComicsClient comicsClient)
		{
			this.kitchenService = kitchenService;
            this.memoryCache = memoryCache;
            this.comicsClient = comicsClient;
		}

        public async Task<IActionResult> Index()
        {
            // TODO: Cache entire view instead
            IEnumerable<ResponsibleForWeek> weeksAndResponsibles;
            // Look for cache key.
            if (!memoryCache.TryGetValue(Keys.WeeksAndResponsibles, out weeksAndResponsibles))
            {
                // Key not in cache, so get data.
                weeksAndResponsibles = await kitchenService.GetWeeksAndResponsibles();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(59));

                // Save data in cache.
                memoryCache.Set(Keys.WeeksAndResponsibles, weeksAndResponsibles, cacheEntryOptions);
            }

            ViewData["WeeksAndResponsibles"] = weeksAndResponsibles.Take(5);
            ViewData["LatestComic"] = await comicsClient.GetLatestComicsAsync();

            // TODO: iPad app to enable proper fullscreen and service discovery

            return View();
        }
    }
}
