using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenResponsibleService.Clients;
using KitchenResponsibleService.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace KitchenResponsibleService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        readonly KitchenService kitchenService;
        readonly IMemoryCache memoryCache;
        readonly ComicsClient comicsClient;
        readonly TelemetryClient telemetryClient;

        public HomeController(KitchenService kitchenService, IMemoryCache memoryCache, ComicsClient comicsClient, TelemetryClient telemetryClient)
        {
            this.kitchenService = kitchenService;
            this.memoryCache = memoryCache;
            this.comicsClient = comicsClient;
            this.telemetryClient = telemetryClient;
        }

        // TODO: Show/Hide button is ridiculously ugly, half transparent last row or something must be better
        public async Task<IActionResult> Index()
        {
#if DEBUG
                return (await GetWebsite()).view;
#else
                return await GetWebsiteFromCacheIfFilled();
#endif
        }

        async Task<IActionResult> GetWebsiteFromCacheIfFilled() {
			if (!memoryCache.TryGetValue(Keys.KitchenResponsibleWebsite, out ViewResult view))
			{
                var site = await GetWebsite();
                view = site.view;
                if (site.cache) {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(59));

                    memoryCache.Set(Keys.KitchenResponsibleWebsite, view, cacheEntryOptions);
                }
			}

            return view;
        }

        async Task<(ViewResult view, bool cache)> GetWebsite() {
            bool shouldCache = await GetPartOfWebsiteData(kitchenService.GetWeeksAndResponsibles(), "WeeksAndResponsibles");
            shouldCache &= await GetPartOfWebsiteData(comicsClient.GetLatestComicAsync(), "LatestComic");
            return (View(), shouldCache);
        } 

        async Task<bool> GetPartOfWebsiteData<T>(Task<T> fetcher, string viewDataName) {
            try
            {
                ViewData[viewDataName] = await fetcher;
                return true;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> { { "Part of website", viewDataName } };
                telemetryClient.TrackException(ex, properties);
                ViewData[viewDataName] = null;
                return false;
            }
        }
    }
}
