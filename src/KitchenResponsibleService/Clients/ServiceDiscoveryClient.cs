using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;
using KitchenResponsibleService.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace KitchenResponsibleService.Clients
{
    public class ServiceDiscoveryClient
    {
        public const string ComicService = "comics-service";

        readonly AppConfiguration configuration;
        readonly HttpClient httpClient;

        public ServiceDiscoveryClient(HttpClient httpClient, AppConfiguration configuration)
		{
            this.httpClient = httpClient;
            this.configuration = configuration;
            configuration.ServiceDiscoveryURL = "http://"  + configuration.ServiceDiscoveryURL + "/api/services/";
        }

        public async Task SetComicServiceURL(string ip = null) {
            string comicsURL;
            if (ip == null) {
                var service = await httpClient.GetStringAsync(configuration.ServiceDiscoveryURL + ComicService);
                comicsURL = JsonConvert.DeserializeObject<Service>(service).IP;
            } else {
                comicsURL = ip;
            }
            
			configuration.ComicsServiceURL =  "http://" + comicsURL + "/api/";
        }
    }
}