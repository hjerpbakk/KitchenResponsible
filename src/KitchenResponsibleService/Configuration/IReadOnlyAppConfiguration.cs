namespace KitchenResponsibleService.Configuration
{
    public interface IReadOnlyAppConfiguration 
    {
        string ComicsServiceURL { get; }
        string ServiceDiscoveryURL { get; }

        string BlobStorageAccessKey { get; }
        string BlobStorageAccountName { get; }
        string BlobStorageEndpointSuffix { get; }

        string ConnectionString { get; }
    }
}