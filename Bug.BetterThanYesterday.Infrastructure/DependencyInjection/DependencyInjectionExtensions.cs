using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Uploads;
using Bug.BetterThanYesterday.Infrastructure.Persistence.CheckIns;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Habits;
using Bug.BetterThanYesterday.Infrastructure.Persistence.PlanMembers;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Bug.BetterThanYesterday.Infrastructure.S3;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddDocumentMappers();
		services.AddRepositories();
		services.AddMongoCollections();
		services.AddScoped<IPresignedUploadUrlGenerator, S3PresignedUploadUrlGenerator>();
		return services;
	}

	public static IServiceCollection AddDocumentMappers(this IServiceCollection services)
	{
		services.AddScoped<IDocumentMapper<CheckIn, CheckInDocument>, CheckInMapper>();
		services.AddScoped<IDocumentMapper<Habit, HabitDocument>, HabitMapper>();
		services.AddScoped<IDocumentMapper<Plan, PlanDocument>, PlanMapper>();
		services.AddScoped<IDocumentMapper<PlanMember, PlanMemberDocument>, PlanMemberMapper>();
		services.AddScoped<IDocumentMapper<User, UserDocument>, UserMapper>();
		return services;
	}

	public static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<ICheckInRepository, CheckInRepository>();
		services.AddScoped<IRepository<CheckIn>>(sp => sp.GetRequiredService<ICheckInRepository>());
		services.AddScoped<IRepository<CheckIn>, Repository<CheckIn, CheckInDocument>>();
		
		services.AddScoped<IHabitRepository, HabitRepository>();
		services.AddScoped<IRepository<Habit>>(sp => sp.GetRequiredService<IHabitRepository>());
		services.AddScoped<IRepository<Habit>, Repository<Habit, HabitDocument>>();

		services.AddScoped<IPlanRepository, PlanRepository>();
		services.AddScoped<IRepository<Plan>>(sp => sp.GetRequiredService<IPlanRepository>());
		services.AddScoped<IRepository<Plan>, Repository<Plan, PlanDocument>>();

		services.AddScoped<IPlanMemberRepository, PlanMemberRepository>();
		services.AddScoped<IRepository<PlanMember>>(sp => sp.GetRequiredService<IPlanMemberRepository>());
		services.AddScoped<IRepository<PlanMember>, Repository<PlanMember, PlanMemberDocument>>();

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRepository<User>>(sp => sp.GetRequiredService<IUserRepository>());
		services.AddScoped<IRepository<User>, Repository<User, UserDocument>>();

		return services;
	}

	public static IServiceCollection AddMongoCollections(this IServiceCollection services)
	{
		services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<CheckInDocument>("checkins"));
		services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<HabitDocument>("habits"));
		services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<PlanDocument>("plans"));
		services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<PlanMemberDocument>("plan_members"));
		services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<UserDocument>("users"));

		return services;
	}
}
