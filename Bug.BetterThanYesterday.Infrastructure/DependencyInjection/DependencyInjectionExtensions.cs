using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.BetterThanYesterday.Infrastructure.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRepository<User>>(sp => sp.GetRequiredService<IUserRepository>());
			return services;
		}
	}
}
