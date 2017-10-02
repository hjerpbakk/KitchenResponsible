namespace KitchenResponsibleService.Configuration
{
    public class AppConfiguration : IReadOnlyAppConfiguration
    {
        public AppConfiguration()
        {
            ComicsServiceURL = "";
        }

        public string ComicsServiceURL { get; set; }
    }
}