using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.BetterThanYesterday.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddModelMappers();
		services.AddUseCases();
		return services;
	}

	public static IServiceCollection AddModelMappers(this IServiceCollection services)
	{
		services.AddScoped<IModelMapper<CheckIn, CheckInModel>, CheckInMapper>();
		services.AddScoped<IModelMapper<Habit, HabitModel>, HabitMapper>();
		services.AddScoped<IModelMapper<Plan, PlanModel>, PlanMapper>();
		services.AddScoped<IModelMapper<PlanParticipant, PlanParticipantModel>, PlanParticipantMapper>();
		services.AddScoped<IModelMapper<User, UserModel>, UserMapper>();
		return services;
	}

	public static IServiceCollection AddUseCases(this IServiceCollection services)
	{
		//services.AddScoped<IUseCase<*CheckInCommand, Result<CheckInModel>>, *ChecUseCase>();
		//services.AddScoped<IUseCase<*HabitCommand, Result<HabitModel>>, *HabiUseCase>();
		//services.AddScoped<IUseCase<*PlanCommand, Result<PlanModel>>, *PlanUseCase>();
		//services.AddScoped<IUseCase<*PlanParticipantCommand, Result<PlanParticipantModel>>, *PlanUseCase>();
		
		services.AddScoped<IUseCase<RegisterUserCommand, Result<UserModel>>, RegisterUserUseCase>();
		services.AddScoped<RegisterUserUseCase>();
		services.AddScoped<IUseCase<ListAllUsersCommand, Result<List<UserModel>>>, ListAllUsersUseCase>();
		services.AddScoped<ListAllUsersUseCase>();

		return services;
	}
}
