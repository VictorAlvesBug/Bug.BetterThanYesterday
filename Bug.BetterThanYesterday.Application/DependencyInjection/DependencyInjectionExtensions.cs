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
		services.AddUserUseCases();
		services.AddHabitUseCases();
		services.AddPlanUseCases();
		services.AddPolicies();
		return services;
	}

	public static IServiceCollection AddUserUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllUsersCommand, IResult>, ListAllUsersUseCase>();
		services.AddScoped<IUseCase<GetUserByIdCommand, IResult>, GetUserByIdUseCase>();
		services.AddScoped<IUseCase<RegisterUserCommand, IResult>, RegisterUserUseCase>();
		services.AddScoped<IUseCase<UpdateUserCommand, IResult>, UpdateUserUseCase>();
		services.AddScoped<IUseCase<DeleteUserCommand, IResult>, DeleteUserUseCase>();
		
		return services;
	}

	public static IServiceCollection AddHabitUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllHabitsCommand, IResult>, ListAllHabitsUseCase>();
		services.AddScoped<IUseCase<GetHabitByIdCommand, IResult>, GetHabitByIdUseCase>();
		services.AddScoped<IUseCase<CreateHabitCommand, IResult>, CreateHabitUseCase>();
		services.AddScoped<IUseCase<UpdateHabitCommand, IResult>, UpdateHabitUseCase>();
		services.AddScoped<IUseCase<DeleteHabitCommand, IResult>, DeleteHabitUseCase>();

		return services;
	}

	public static IServiceCollection AddPlanUseCases(this IServiceCollection services)
	{
		services.AddScoped<IUseCase<ListAllPlansCommand, IResult>, ListAllPlansUseCase>();
		services.AddScoped<IUseCase<GetPlanByIdCommand, IResult>, GetPlanByIdUseCase>();
		services.AddScoped<IUseCase<ListPlansByHabitIdCommand, IResult>, ListPlansByHabitIdUseCase>();
		services.AddScoped<IUseCase<CreatePlanCommand, IResult>, CreatePlanUseCase>();
		services.AddScoped<IUseCase<UpdatePlanStatusCommand, IResult>, UpdatePlanStatusUseCase>();
		services.AddScoped<IUseCase<CancelPlanCommand, IResult>, CancelPlanUseCase>();

		return services;
	}

	public static IServiceCollection AddPolicies(this IServiceCollection services)
	{
		services.AddScoped<IHabitDeletionPolicy, HabitDeletionPolicy>();

		return services;
	}
}
