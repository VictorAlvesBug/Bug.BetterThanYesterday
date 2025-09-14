using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.BetterThanYesterday.Infrastructure.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			services.AddSingleton<IUserRepository, UserRepository>();
			return services;
		}
	}
}
