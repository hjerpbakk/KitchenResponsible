namespace KitchenResponsibleService.Configuration
{
    public interface IServiceDiscoveryConfiguration
    {
        string ServiceDiscoveryUrl { get; }
        string ApiKey { get; }
    }
}
