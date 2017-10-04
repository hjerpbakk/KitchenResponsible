using System;
using System.Net.Http;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;

namespace KitchenResponsibleService.Clients
{
    public class ComicsClient
    {
        readonly HttpClient httpClient;
        readonly IReadOnlyAppConfiguration readOnlyAppConfiguration;      

        public ComicsClient(HttpClient httpClient, IReadOnlyAppConfiguration readOnlyAppConfiguration)
        {
            this.httpClient = httpClient;
            this.readOnlyAppConfiguration = readOnlyAppConfiguration;
        }

        string ServiceURL { get { return readOnlyAppConfiguration.ComicsServiceURL + "comics/"; } }
        
        public async Task<string> GetLatestComicAsync() => 
            await httpClient.GetStringAsync(ServiceURL);
    }
}