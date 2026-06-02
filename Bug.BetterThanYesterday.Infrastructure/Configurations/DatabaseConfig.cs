using Bug.BetterThanYesterday.Domain.Configurations;

namespace Bug.BetterThanYesterday.Infrastructure.Configurations;

public class DatabaseConfig : IDatabaseConfig
{
	public string ConnectionString { get; set; }
	public string DatabaseName { get; set; }
	public string TestDatabaseName { get; set; }
}
