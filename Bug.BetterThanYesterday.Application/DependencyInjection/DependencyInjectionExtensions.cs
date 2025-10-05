using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.BetterThanYesterday.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{

		services.AddUseCases();
		services.AddPolicies();
		return services;
	}

	private static IServiceCollection AddUseCases(this IServiceCollection services)
	{
		// Registrar todos os IUseCase<> concretos
		services.Scan(scan => scan
			.FromApplicationDependencies()
			.AddClasses(c => c.AssignableTo(typeof(IUseCase<>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		// Decorar TODOS os IUseCase<> com o seu decorator
		services.Decorate(typeof(IUseCase<>), typeof(ExceptionToResultDecorator<>));

		return services;
	}

	private static IServiceCollection AddPolicies(this IServiceCollection services)
	{
		services.AddScoped<IHabitDeletionPolicy, HabitDeletionPolicy>();

		return services;
	}
}
