namespace KitchenResponsibleService.Configuration
{
    public class AppConfiguration : IBlobStorageConfiguration, IComicsConfiguration
    {
        public string ServiceDiscoveryURL { get; set; }

        public string ComicsServiceURL { get; set; }
        public string ComicsServiceName { get; set; }

        public string BlobStorageConnectionString { get; set; }
    }
}