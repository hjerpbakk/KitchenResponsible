using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;
using Microsoft.ApplicationInsights;

namespace KitchenResponsibleService.Clients
{
    public class ComicsClient
    {
        readonly HttpClient httpClient;
        readonly IComicsConfiguration comicsConfiguration;  
        readonly TelemetryClient telemetryClient;

        public ComicsClient(HttpClient httpClient, IComicsConfiguration comicsConfiguration, TelemetryClient telemetryClient)
        {
            this.httpClient = httpClient;
            this.comicsConfiguration = comicsConfiguration;
            this.telemetryClient = telemetryClient;
        }

        string ServiceURL { get { return comicsConfiguration.ComicsServiceURL + "comics/"; } }

        public async Task<string> GetLatestComicAsync() {
            try
            {
                return await httpClient.GetStringAsync(ServiceURL);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> { { "Could not get comic", ServiceURL ?? "ServiceURL was null" } };
                telemetryClient.TrackException(ex, properties);
                throw;
            }
        }
            
    }
}