using Bug.BetterThanYesterday.Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.BetterThanYesterday.Infrastructure.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		/*public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<IDatabaseConfig>(
	configuration.GetSection("DatabaseConfig"));
			builder.Services.AddSingleton<IDatabaseConfig>(sp => sp.GetRequiredService<IOptions<>>());
		}*/
	}
}
