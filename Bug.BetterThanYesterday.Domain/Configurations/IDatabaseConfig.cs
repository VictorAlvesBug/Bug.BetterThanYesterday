namespace Bug.BetterThanYesterday.Domain.Configurations;

public interface IDatabaseConfig
{
	string DatabaseName { get; set; }
	string TestDatabaseName { get; set; }
}
