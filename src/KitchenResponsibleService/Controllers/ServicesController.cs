using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.ServiceDiscovery.Client;
using KitchenResponsibleService.Clients;
using KitchenResponsibleService.Configuration;
using KitchenResponsibleService.Model;
using Microsoft.AspNetCore.Mvc;

namespace KitchenResponsibleService.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        readonly ServiceDiscoveryClient serviceDiscoveryClient;
        readonly IComicsConfiguration configuration;
        
        public ServicesController(ServiceDiscoveryClient serviceDiscoveryClient, IComicsConfiguration configuration) {
            this.serviceDiscoveryClient = serviceDiscoveryClient;
            this.configuration = configuration;
        }

        [HttpPost]
        public string Post([FromBody]Service service)
        {
            if (service.Name != configuration.ComicsServiceName) {
                return $"Only interested in {configuration.ComicsServiceName}";
            }

            configuration.ComicsServiceURL = service.IP;
            return $"Changed {service.Name} IP to {service.IP}";
        }
    }
}
