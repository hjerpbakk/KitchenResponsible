using System;
using System.Net.Http;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;

namespace KitchenResponsibleService.Clients
{
    public class ComicsClient
    {
        readonly HttpClient httpClient;
        readonly ServiceDiscoveryClient serviceDiscoveryClient;
        readonly IReadOnlyAppConfiguration readOnlyAppConfiguration;

        string ServiceURL => readOnlyAppConfiguration.ComicsServiceURL + "api/";

        public ComicsClient(HttpClient httpClient, ServiceDiscoveryClient serviceDiscoveryClient, IReadOnlyAppConfiguration readOnlyAppConfiguration)
        {
            this.httpClient = httpClient;
            this.serviceDiscoveryClient = serviceDiscoveryClient;
            this.readOnlyAppConfiguration = readOnlyAppConfiguration;
        }
        
        public async Task<string> GetLatestComicAsync() {
            // TODO: Change when BlobStorage supports push
            try
            {
                // TODO: Need to fail faster when ip is wrong
                return await GetLatestComicsAsync();
            }
            catch (Exception)
            {
                await serviceDiscoveryClient.SetComicServiceURL();
                return await GetLatestComicsAsync();
            }
        }

        async Task<string> GetLatestComicsAsync() => await httpClient.GetStringAsync(ServiceURL + "comics");
    }
}