using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Habits.GetHabitById;
using Bug.BetterThanYesterday.Application.Habits.ListAllHabits;
using Bug.BetterThanYesterday.Application.Habits.UpdateHabit;
using Bug.BetterThanYesterday.Application.Plans.CancelPlan;
using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans.GetPlanById;
using Bug.BetterThanYesterday.Application.Plans.ListAllPlans;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;
using Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users.DeleteUser;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Application.Users.UpdateUser;
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
