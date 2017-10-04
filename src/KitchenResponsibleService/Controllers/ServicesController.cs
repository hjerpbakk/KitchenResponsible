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
        public async Task<string> Post([FromBody]Service service)
        {
            if (service.Name != ServiceDiscoveryClient.ComicService) {
                return $"Only interested in {ServiceDiscoveryClient.ComicService}";
            }

            await serviceDiscoveryClient.SetComicServiceURL(service.IP);
            return $"Changed {service.Name} IP to {service.IP}";
        }
    }
}
