namespace Bug.BetterThanYesterday.Domain.Configurations;

public interface IDatabaseConfig
{
	string ConnectionString { get; set; }
	string DatabaseName { get; set; }
	string TestDatabaseName { get; set; }
}
