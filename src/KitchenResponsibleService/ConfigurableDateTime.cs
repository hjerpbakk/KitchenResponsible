using System;
namespace KitchenResponsibleService
{
    public static class ConfigurableDateTime
    {
		public static DateTime? CurrentTime { private get; set; }

		public static DateTime UtcNow => CurrentTime ?? DateTime.UtcNow;
    }
}
