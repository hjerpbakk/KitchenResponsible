using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
