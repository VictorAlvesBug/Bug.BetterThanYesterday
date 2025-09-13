using Bug.BetterThanYesterday.Domain.Configurations;

namespace Bug.BetterThanYesterday.Infrastructure.Configurations
{
	public class DatabaseConfig : IDatabaseConfig
	{
		public string DatabaseName { get; set; }
		public string ConnectionString { get; set; }
	}
}
