using Newtonsoft.Json;

namespace KitchenResponsibleService.Configuration
{
    public class AppConfiguration : IReadOnlyAppConfiguration
    {
        public string ComicsServiceURL { get; set; }
        public string ServiceDiscoveryURL { get; set; }

        public string BlobStorageAccessKey { get; set; }
        public string BlobStorageAccountName { get; set; }
        public string BlobStorageEndpointSuffix { get; set; }

        [JsonIgnore]
        public string ConnectionString => 
            $"DefaultEndpointsProtocol=https;AccountName={BlobStorageAccountName};AccountKey={BlobStorageAccessKey};EndpointSuffix={BlobStorageEndpointSuffix}";
    }
}