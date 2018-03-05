namespace KitchenResponsibleService.Configuration
{
    public class AppConfiguration : IBlobStorageConfiguration, IComicsConfiguration, IServiceDiscoveryConfiguration
    {
        public string ServiceDiscoveryUrl { get; set; }
        public string ApiKey { get; set; }

        public string ComicsServiceURL { get; set; }
        public string ComicsServiceName { get; set; }

        public string BlobStorageConnectionString { get; set; }
    }
}