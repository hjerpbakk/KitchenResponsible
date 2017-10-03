using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KitchenResponsibleService.Clients;
using KitchenResponsibleService.Model;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        readonly ServiceDiscoveryClient serviceDiscoveryClient;
        
        public ServicesController(ServiceDiscoveryClient serviceDiscoveryClient) {
            this.serviceDiscoveryClient = serviceDiscoveryClient;
        }

        [HttpPost]
        public async Task Post([FromBody]Service service)
        {
            if (service.Name != ServiceDiscoveryClient.ComicService) {
                return;
            }

            await serviceDiscoveryClient.SetComicServiceURL(service.Name);
        }
    }
}
