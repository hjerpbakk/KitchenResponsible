using KitchenResponsible.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsible.Controllers {
    [Route("api/[controller]")]
    public class KitchenResponsibleController : Controller {
        readonly IKitchenResponsibleService kitchenResponsibleService;       

        public KitchenResponsibleController(IKitchenResponsibleService kitchenResponsibleService) {
            this.kitchenResponsibleService = kitchenResponsibleService;
        }

        // GET api/kitchenresponsible
        [HttpGet]
        public IActionResult Get() {
            return Ok(kitchenResponsibleService.GetWeeksWithResponsible());
        }
    }
}