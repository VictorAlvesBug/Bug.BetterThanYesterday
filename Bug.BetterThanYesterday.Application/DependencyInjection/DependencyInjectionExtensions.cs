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
		// 1) Registrar todos os IUseCase<,> concretos
		services.Scan(scan => scan
			.FromApplicationDependencies()
			.AddClasses(c => c.AssignableTo(typeof(IUseCase<>)))
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		// 2) Decorar TODOS os IUseCase<,> com o seu decorator
		services.Decorate(typeof(IUseCase<>), typeof(ExceptionToResultDecorator<>));

		//services.AddUserUseCases();
		//services.AddHabitUseCases();
		//services.AddPlanUseCases();

		return services;
	}

	private static IServiceCollection AddUserUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllUsersCommand>, ListAllUsersUseCase>();
		services.AddScoped<IUseCase<GetUserByIdCommand>, GetUserByIdUseCase>();
		services.AddScoped<IUseCase<RegisterUserCommand>, RegisterUserUseCase>();
		services.AddScoped<IUseCase<UpdateUserCommand>, UpdateUserUseCase>();
		services.AddScoped<IUseCase<DeleteUserCommand>, DeleteUserUseCase>();
		
		return services;
	}

	private static IServiceCollection AddHabitUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllHabitsCommand>, ListAllHabitsUseCase>();
		services.AddScoped<IUseCase<GetHabitByIdCommand>, GetHabitByIdUseCase>();
		services.AddScoped<IUseCase<CreateHabitCommand>, CreateHabitUseCase>();
		services.AddScoped<IUseCase<UpdateHabitCommand>, UpdateHabitUseCase>();
		services.AddScoped<IUseCase<DeleteHabitCommand>, DeleteHabitUseCase>();

		return services;
	}

	private static IServiceCollection AddPlanUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllPlansCommand>, ListAllPlansUseCase>();
		services.AddScoped<IUseCase<GetPlanByIdCommand>, GetPlanByIdUseCase>();
		services.AddScoped<IUseCase<ListPlansByHabitIdCommand>, ListPlansByHabitIdUseCase>();
		services.AddScoped<IUseCase<CreatePlanCommand>, CreatePlanUseCase>();
		services.AddScoped<IUseCase<UpdatePlanStatusCommand>, UpdatePlanStatusUseCase>();
		services.AddScoped<IUseCase<CancelPlanCommand>, CancelPlanUseCase>();

		return services;
	}

	private static IServiceCollection AddPolicies(this IServiceCollection services)
	{
		services.AddScoped<IHabitDeletionPolicy, HabitDeletionPolicy>();

		return services;
	}
}
