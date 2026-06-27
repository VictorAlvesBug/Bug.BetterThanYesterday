using Bug.BetterThanYesterday.API;
using Bug.BetterThanYesterday.API.Configurations;
using Bug.BetterThanYesterday.Application.DependencyInjection;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseConfig>(
builder.Configuration.GetSection(nameof(DatabaseConfig)));

builder.Services.Configure<AwsConfig>(
	builder.Configuration.GetSection(nameof(AwsConfig)));

builder.Services.AddSingleton<IConfigureOptions<AwsConfig>, AwsConfigEnvironmentConfigurer>();

builder.Services.AddSingleton<IAwsConfig>(sp =>
	sp.GetRequiredService<IOptions<AwsConfig>>().Value);

builder.Services.AddSingleton(sp =>
{
	var dbConfig = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;
	return (IMongoClient)new MongoClient(dbConfig.ConnectionString);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
	var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
	var client = sp.GetRequiredService<IMongoClient>();
	var dbConfig = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;

	var path = httpContextAccessor.HttpContext?.Request.Path.Value ?? string.Empty;

	var databaseName = path.StartsWith("/testapi", StringComparison.OrdinalIgnoreCase)
		? dbConfig.TestDatabaseName
		: dbConfig.DatabaseName;

	return client.GetDatabase(databaseName);
});

//builder.Services.AddTransient<IMiddleware, ExceptionHandlingMiddleware>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowLocalhost",
		builder =>
		{
			builder.SetIsOriginAllowed(origin =>
				{
					if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
						return false;

					if (uri.Host is "localhost" or "127.0.0.1")
						return true;

					return uri.Host.StartsWith("192.168.", StringComparison.Ordinal);
				})
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*

TODO - Criar back e front para o ranking por plano
TODO - Alterar lista de checkins para permitir avaliação na tela inicial do plano
TODO - Testar uso da folga
TODO - Definir o que vai ficar na tela de 'configurações' e 'sobre nós'
TODO - Implementar entrada num plano via link de convite
TODO - Implementar recarregamento das telas ao puxar de cima para baixo
TODO - Ajustar para persistir a imagem apenas quando o checkin for criado

######### TODO - End-to-end tests to implement via CursorAI #########

- GetHabitById with a non-guid habitId
- GetHabitById with a valid guid, but not registered
- GetHabitById with a registered habitId
- ListHabitsByFilter without any parameter
- ListHabitsByFilter with a non-registered name
- ListHabitsByFilter with a registered name
- AddHabit with an empty body
- AddHabit with an white-space name
- AddHabit with an valid name
- GetUserById with a non-guid userId
- GetUserById with a valid guid, but not registered
- GetUserById with a registered userId
- ListUsersByFilter without any parameter
- ListUsersByFilter with an invalid email
- ListUsersByFilter with a non-registered email
- ListUsersByFilter with a registered email
- AddUser with an empty body
- AddUser without name
- AddUser with an white-space name
- AddUser without email
- AddUser with an white-space email
- AddUser with an invalid email
- AddUser without nickname
- AddUser with an white-space nickname
- AddUser without phone number
- AddUser with an white-space phone number
- AddUser with an invalid phone number
- AddUser without pix key type
- AddUser with an invalid numeric pix key type
- AddUser with an invalid text pix key type
- AddUser without pix key
- AddUser with an invalid pix key
- AddUser with valid fields
- GetPlanById with a non-guid planId
- GetPlanById with a valid guid, but not registered
- GetPlanById with a registered planId
- ListPlansByFilter without any parameter
- ListPlansByFilter with non-guid ownerId
- ListPlansByFilter with non-registered ownerId
- ListPlansByFilter with registered ownerId that has no plans
- ListPlansByFilter with registered ownerId that has plans
- ListPlansByFilter with non-guid habitId
- ListPlansByFilter with non-registered habitId
- ListPlansByFilter with registered habitId that has no plans
- ListPlansByFilter with registered habitId that has plans
- ListPlansByFilter with an invalid numeric status
- ListPlansByFilter with an invalid text status
- ListPlansByFilter with an invalid numeric type
- ListPlansByFilter with an invalid text type
- ListPlansByFilter with all field valid
- AddPlan with an empty body
- AddPlan without ownerId
- AddPlan with an non-guid ownerId
- AddPlan with an non-registered ownerId
- AddPlan with an registered ownerId
- AddPlan without habitId
- AddPlan with an non-guid habitId
- AddPlan with an non-registered habitId
- AddPlan with an registered habitId
- AddPlan without a description
- AddPlan with a description
- AddPlan without startsAt
- AddPlan with startsAt before today
- AddPlan without endsAt
- AddPlan with endsAt before startsAt
- AddPlan with endsAt before today
- AddPlan without type
- AddPlan with invalid numeric type
- AddPlan with invalid text type
- AddPlan with all field valid
- GetPlanMemberDetails with a non-guid planId
- GetPlanMemberDetails with a non-registered planId
- GetPlanMemberDetails with a registered, but without members planId
- GetPlanMemberDetails with a non-guid userId
- GetPlanMemberDetails with a non-registered userId
- GetPlanMemberDetails with a registered, but without plans userId
- GetPlanMemberDetails with registered planId and userId related
- GetPlanWithMembersByPlanId with a non-guid planId
- GetPlanWithMembersByPlanId with a non-registered planId
- GetPlanWithMembersByPlanId with a registered, but without members planId
- GetPlanWithMembersByPlanId with a registered, and with members planId
- GetUserWithPlansByUserId with a non-guid userId
- GetUserWithPlansByUserId with a non-registered userId
- GetUserWithPlansByUserId with a registered, but without plans userId
- GetUserWithPlansByUserId with a registered, and with plans userId
- AddUserToPlan with a non-guid planId
- AddUserToPlan with a non-registered planId
- AddUserToPlan with a non-guid userId
- AddUserToPlan with a non-registered userId
- AddUserToPlan with registered planId and userId, not related yet
- AddUserToPlan with registered planId and userId, already related
- AddUserToPlan with registered planId and userId, and not-started plan
- AddUserToPlan with registered planId and userId, and running plan
- AddUserToPlan with registered planId and userId, and finished plan
- AddUserToPlan with registered planId and userId, and cancelled plan
- RemoveUserFromPlan with a non-guid planId
- RemoveUserFromPlan with a non-registered planId
- RemoveUserFromPlan with a non-guid userId
- RemoveUserFromPlan with a non-registered userId
- RemoveUserFromPlan with registered planId and userId, not related
- RemoveUserFromPlan with registered planId and userId related and active
- RemoveUserFromPlan with registered planId and userId related and blocked
- RemoveUserFromPlan with registered planId and userId, and not-started plan
- RemoveUserFromPlan with registered planId and userId, and running plan
- RemoveUserFromPlan with registered planId and userId, and finished plan
- RemoveUserFromPlan with registered planId and userId, and cancelled plan
- BlockUserInThePlan with a non-guid planId
- BlockUserInThePlan with a non-registered planId
- BlockUserInThePlan with a non-guid userId
- BlockUserInThePlan with a non-registered userId
- BlockUserInThePlan with registered planId and userId, not related
- BlockUserInThePlan with registered planId and userId related and active
- BlockUserInThePlan with registered planId and userId related and blocked
- BlockUserInThePlan with registered planId and userId, and not-started plan
- BlockUserInThePlan with registered planId and userId, and running plan
- BlockUserInThePlan with registered planId and userId, and finished plan
- BlockUserInThePlan with registered planId and userId, and cancelled plan
- UnblockUserInThePlan with a non-guid planId
- UnblockUserInThePlan with a non-registered planId
- UnblockUserInThePlan with a non-guid userId
- UnblockUserInThePlan with a non-registered userId
- UnblockUserInThePlan with registered planId and userId, not related
- UnblockUserInThePlan with registered planId and userId related and active
- UnblockUserInThePlan with registered planId and userId related and blocked
- UnblockUserInThePlan with registered planId and userId, and not-started plan
- UnblockUserInThePlan with registered planId and userId, and running plan
- UnblockUserInThePlan with registered planId and userId, and finished plan
- UnblockUserInThePlan with registered planId and userId, and cancelled plan
- GetCheckInById with a non-guid checkIn
- GetCheckInById with a valid guid, but not registered
- GetCheckInById with a registered checkIn
- ListCheckInsByFilter with a non-guid planId
- ListCheckInsByFilter with a non-registered planId
- ListCheckInsByFilter with a registered planId, without any checkIns related
- ListCheckInsByFilter with a registered planId, with checkIns related
- ListCheckInsByFilter only with userId
- ListCheckInsByFilter only with date
- ListCheckInsByFilter only with status
- ListCheckInsByFilter with a registered planId and non-guid userId
- ListCheckInsByFilter with a registered planId and non-registered userId
- ListCheckInsByFilter with a registered planId and registered userId, but not related
- ListCheckInsByFilter with a registered planId and registered userId, related via planMembers, but with no checkIns
- ListCheckInsByFilter with a registered planId and registered userId, related and with some checkIns
- ListCheckInsByFilter with a registered planId and date
- ListCheckInsByFilter with a registered planId and invalid numeric status
- ListCheckInsByFilter with a registered planId and invalid text status
- ListCheckInsByFilter with a registered planId and valid numberic status
- ListCheckInsByFilter with a registered planId and valid text status
- AddCheckIn with empty body
- AddCheckIn without planId
- AddCheckIn with non-guid planId
- AddCheckIn with non-registered planId
- AddCheckIn with registered planId
- AddCheckIn without userId
- AddCheckIn with non-guid userId
- AddCheckIn with non-registered userId
- AddCheckIn with registered userId
- AddCheckIn without title
- AddCheckIn with white space title
- AddCheckIn without photoUrl
- AddCheckIn with empty photoUrl
- AddCheckIn without date
- AddCheckIn with planId and userId not related via planMembers
- AddCheckIn with user blocked in the plan
- AddCheckIn with date before yesterday
- AddCheckIn with date after today
- AddCheckIn with date before plan startsAt
- AddCheckIn with date after plan endsAt
- AddCheckIn with not-started plan
- AddCheckIn with running plan
- AddCheckIn with finished plan
- AddCheckIn with cancelled plan
- AddCheckIn with same fields already registered



- ReviewCheckIn with empty body
- ReviewCheckIn without checkInId
- ReviewCheckIn with non-guid checkInId
- ReviewCheckIn with non-registered checkInId
- ReviewCheckIn with registered checkInId
- ReviewCheckIn without reviewerId
- ReviewCheckIn with non-guid reviewerId
- ReviewCheckIn with non-registered reviewerId
- ReviewCheckIn with registered reviewerId, not related with the plan
- ReviewCheckIn with registered reviewerId, related with the plan
- ReviewCheckIn with registered reviewerId, to his own checkIn
- ReviewCheckIn without status
- ReviewCheckIn with invalid numeric status
- ReviewCheckIn with invalid text status
- ReviewCheckIn with valid numeric status
- ReviewCheckIn with valid text status
- ReviewCheckIn without date
- ReviewCheckIn with date before checkIn createdAt
- ReviewCheckIn with date after checkIn date+1
- ReviewCheckIn for checkIn from a not-started plan
- ReviewCheckIn for checkIn from a running plan
- ReviewCheckIn for checkIn from a finished plan
- ReviewCheckIn for checkIn from a cancelled plan
- ReviewCheckIn for checkIn already reviewed by this reviewer
- ReviewCheckIn for a non-pending checkIn

######### Cenários de teste #########

# Testes do UseCase

## CheckIn

- CheckInId (PlanId,UserId,Date,Index=0)
- PlanId
- UserId
- Date
- Index
- Title
- Description

--> GetCheckInDetailsUseCase
(planId,userId,date,index=0)
- Valid
- NotFoundCheckInId


--> GetPlanWithCheckInsByPlanId
(planId)
- Valid
- PlanWithoutCheckIns
- NotFoundPlanId


--> GetPlanUserWithCheckInsByPlanIdAndUserId
(planId,userId)
- Valid
- PlanUserWithoutCheckIns
- UserIsNotInThisPlan
- NotFoundPlanId
- NotFoundUserId


--> AddCheckIn
(planId,userId,date,index=0,title,description)
- Valid
- NotFoundPlanId
- NotFoundUserId
- DateOutOfAllowedRange
- IndexOutOfAllowedRange
- DuplicatedCheckIn
- UserIsNotInThisPlan
- PlanIsNotRunning
- UserIsBlocked

# Testes da Controller

## ListAllUsers()
 - Test_ListAllUsersUseCase_Valid_ShouldReturn200();
 - Test_ListAllUsersUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_ListAllUsersUseCase_InvalidUserIdFromDb_ShouldReturn422();
 - Test_ListAllUsersUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_ListAllUsersUseCase_EmptyEmailFromDb_ShouldReturn422();
 - Test_ListAllUsersUseCase_InvalidEmailFromDb_ShouldReturn422();
 - Test_ListAllUsersUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_ListAllUsersUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## GetUserById(id)
 - Test_GetUserByIdUseCase_Valid_ShouldReturn200();
 - Test_GetUserByIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_GetUserByIdUseCase_InvalidUserId_ShouldReturn400();
 - Test_GetUserByIdUseCase_NotFoundUserId_ShouldReturn404();
 - Test_GetUserByIdUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_GetUserByIdUseCase_EmptyEmailFromDb_ShouldReturn422();
 - Test_GetUserByIdUseCase_InvalidEmailFromDb_ShouldReturn422();
 - Test_GetUserByIdUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_GetUserByIdUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## RegisterUser(name, email)
 - Test_RegisterUserUseCase_Valid_ShouldReturn201();
 - Test_RegisterUserUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_RegisterUserUseCase_EmptyName_ShouldReturn400();
 - Test_RegisterUserUseCase_EmptyEmail_ShouldReturn400();
 - Test_RegisterUserUseCase_InvalidEmail_ShouldReturn400();
 - Test_RegisterUserUseCase_DuplicatedEmail_ShouldReturn400();
 
 ## UpdateUser(id, name, email)
 - Test_UpdateUserUseCase_Valid_ShouldReturn200();
 - Test_UpdateUserUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_UpdateUserUseCase_InvalidUserId_ShouldReturn400();
 - Test_UpdateUserUseCase_NotFoundUserId_ShouldReturn404();
 - Test_UpdateUserUseCase_EmptyNameAndEmail_ShouldReturn400();
 - Test_UpdateUserUseCase_DuplicatedEmail_ShouldReturn400();
 - Test_UpdateUserUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_UpdateUserUseCase_EmptyEmailFromDb_ShouldReturn422();
 - Test_UpdateUserUseCase_InvalidEmailFromDb_ShouldReturn422();
 - Test_UpdateUserUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_UpdateUserUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## DeleteUser(id)
 - Test_DeleteUserUseCase_Valid_ShouldReturn204();
 - Test_DeleteUserUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_DeleteUserUseCase_InvalidUserId_ShouldReturn400();
 - Test_DeleteUserUseCase_NotFoundUserId_ShouldReturn404();
 - Test_DeleteUserUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_DeleteUserUseCase_EmptyEmailFromDb_ShouldReturn422();
 - Test_DeleteUserUseCase_InvalidEmailFromDb_ShouldReturn422();
 - Test_DeleteUserUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_DeleteUserUseCase_InvalidCreatedAtFromDb_ShouldReturn422(); 

---

 ## ListAllHabits()
 - Test_ListAllHabitsUseCase_Valid_ShouldReturn200();
 - Test_ListAllHabitsUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_ListAllHabitsUseCase_InvalidHabitIdFromDb_ShouldReturn422();
 - Test_ListAllHabitsUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_ListAllHabitsUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_ListAllHabitsUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## GetHabitById(id)
 - Test_GetHabitByIdUseCase_Valid_ShouldReturn200();
 - Test_GetHabitByIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_GetHabitByIdUseCase_InvalidHabitId_ShouldReturn400();
 - Test_GetHabitByIdUseCase_NotFoundHabitId_ShouldReturn404();
 - Test_GetHabitByIdUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_GetHabitByIdUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_GetHabitByIdUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## CreateHabit(name)
 - Test_CreateHabitUseCase_Valid_ShouldReturn201();
 - Test_CreateHabitUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_CreateHabitUseCase_EmptyName_ShouldReturn400();
 - Test_CreateHabitUseCase_DuplicatedName_ShouldReturn400();
 
 ## UpdateHabit(id, name)
 - Test_UpdateHabitUseCase_Valid_ShouldReturn200();
 - Test_UpdateHabitUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_UpdateHabitUseCase_InvalidHabitId_ShouldReturn400();
 - Test_UpdateHabitUseCase_NotFoundHabitId_ShouldReturn404();
 - Test_UpdateHabitUseCase_EmptyName_ShouldReturn400();
 - Test_UpdateHabitUseCase_DuplicatedName_ShouldReturn400();
 - Test_UpdateHabitUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_UpdateHabitUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_UpdateHabitUseCase_InvalidCreatedAtFromDb_ShouldReturn422();
 
 ## DeleteHabit(id)
 - Test_DeleteHabitUseCase_Valid_ShouldReturn204();
 - Test_DeleteHabitUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_DeleteHabitUseCase_InvalidHabitId_ShouldReturn400();
 - Test_DeleteHabitUseCase_NotFoundHabitId_ShouldReturn404();
 - Test_DeleteHabitUseCase_NonCancelledPlansRelated_ShouldReturn400();
 - Test_DeleteHabitUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_DeleteHabitUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_DeleteHabitUseCase_InvalidCreatedAtFromDb_ShouldReturn422();

---

// Definir testes de 422 (recursos inválidos vindos do banco) de planos
 
 ## ListPlansByFilter()
 - Test_ListPlansByFilterUseCase_Valid_ShouldReturn200();
 - Test_ListPlansByFilterUseCase_InvalidOrMissingToken_ShouldReturn401();
 
 ## GetPlanById(id)
 - Test_GetPlanByIdUseCase_Valid_ShouldReturn200();
 - Test_GetPlanByIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_GetPlanByIdUseCase_InvalidPlanId_ShouldReturn400();
 - Test_GetPlanByIdUseCase_NotFoundPlanId_ShouldReturn404();
 
 ## ListPlansByHabitId(habitId)
 - Test_ListPlansByHabitIdUseCase_Valid_ShouldReturn200();
 - Test_ListPlansByHabitIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_ListPlansByHabitIdUseCase_InvalidHabitId_ShouldReturn400();
 - Test_ListPlansByHabitIdUseCase_NotFoundHabitId_ShouldReturn404();
 
 ## CreatePlan(habitId, description, startsAt, endsAt, typeId)
 - Test_CreatePlanUseCase_Valid_ShouldReturn201();
 - Test_CreatePlanUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_CreatePlanUseCase_InvalidHabitId_ShouldReturn400();
 - Test_CreatePlanUseCase_NotFoundHabitId_ShouldReturn404();
 - Test_CreatePlanUseCase_EmptyStartsAt_ShouldReturn400();
 - Test_CreatePlanUseCase_StartsAtBeforeToday_ShouldReturn400();
 - Test_CreatePlanUseCase_EmptyEndsAt_ShouldReturn400();
 - Test_CreatePlanUseCase_EndsAtBeforeStartsAt_ShouldReturn400();
 - Test_CreatePlanUseCase_EmptyTypeId_ShouldReturn400();
 - Test_CreatePlanUseCase_InvalidTypeId_ShouldReturn400();
 
 ## UpdatePlanStatus(id, statusId)
 - Test_UpdatePlanStatusUseCase_Valid_ShouldReturn200();
 - Test_UpdatePlanStatusUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_UpdatePlanStatusUseCase_InvalidPlanId_ShouldReturn400();
 - Test_UpdatePlanStatusUseCase_NotFoundPlanId_ShouldReturn404();
 - Test_UpdatePlanStatusUseCase_EmptyStatusId_ShouldReturn400();
 - Test_UpdatePlanStatusUseCase_InvalidStatusId_ShouldReturn400();
 - Test_UpdatePlanStatusUseCase_StatusIdNotAllowed_ShouldReturn400();
 
 ## CancelPlan(id)
 - Test_CancelPlanUseCase_Valid_ShouldReturn200();
 - Test_CancelPlanUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_CancelPlanUseCase_InvalidPlanId_ShouldReturn400();
 - Test_CancelPlanUseCase_NotFoundPlanId_ShouldReturn404();



########################################  Versão 1  ########################################

users (Collection)
- Id (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Name (Ex: John Doe)
- Email (Ex: johndoe@gmail.com) (unique)
- CreatedAt (Ex: 2024-01-01)
> ListAll
> GetById
> Register
> Login


habits (Collection)
- Id (Ex: cd383069fbd840b3ad503447cf9e488d)
- Name (Ex: Workout)
- CreatedAt (Ex: 2024-01-01)
> ListAll
> GetById
> Create
> Update


plans (Collection)
- Id (Ex: b6a049375f6f4445a4a884b02eedeac0)
- HabitId (Ex: cd383069fbd840b3ad503447cf9e488d)
- Description (Ex: Workout 5 times a week)
- StartsAt (Ex: 2024-01-01)
- EndsAt (Ex: 2024-12-31)
- Status (NotStarted,Running,Finished,Cancelled)
- Type (Public,Private)
- CreatedAt (Ex: 2024-01-01)
> ListAll
> GetById
> ListByHabitId
> Create
> UpdateStatus
> Cancel


plan_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
> GetById
> ListByPlanId
> ListByUserId
> AddUserToPlan
> RemoveUserFromPlan
> BlockUserInThePlan
> UnblockUserInThePlan

checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Date (Ex: 2024-01-05)
- Title (Ex: Let's call it a day)
- Description (Ex: Today was rough)
> GetById
> ListByPlanId
> ListByPlanIdAndUserId
> Check


########################################  Versão 2  ########################################

users (Collection)
- Id (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Name (Ex: John Doe)
- Email (Ex: johndoe@gmail.com) (unique)
- CreatedAt (Ex: 2024-01-01)


habits (Collection)
- Id (Ex: cd383069fbd840b3ad503447cf9e488d)
- Name (Ex: Workout)
- CreatedAt (Ex: 2024-01-01)


plans (Collection)
- Id (Ex: b6a049375f6f4445a4a884b02eedeac0)
- HabitId (Ex: cd383069fbd840b3ad503447cf9e488d)
- Description (Ex: Workout 5 times a week)
- StartsAt (Ex: 2024-01-01)
- EndsAt (Ex: 2024-12-31)
- Frequency: (Ex: { Count: 5, Per: "WEEK" })
- RestsPerPeriod (Ex: 2)
- Status (NotStarted,Running,Finished,Cancelled)
- Type (Public,Private)
- CreatedAt (Ex: 2024-01-01)


plan_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
- RestsAvailable (Number of rests left for the current period)


checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Role (CheckedIn,Rest)
- Date (Ex: 2024-01-05)
- Title (Ex: Let's call it a day)
- Description (Ex: Today was rough)
- Evidence (Image Link)


########################################  Versão 3  ########################################

users (Collection)
- Id (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Name (Ex: John Doe)
- Email (Ex: johndoe@gmail.com) (unique)
- PixKey (Ex: { Type: "E-MAIL", Value: "johndoe@gmail.com" }) (Dados Sensiveis: Field Level Encryption)
- CreatedAt (Ex: 2024-01-01)


habits (Collection)
- Id (Ex: cd383069fbd840b3ad503447cf9e488d)
- Name (Ex: Workout)
- CreatedAt (Ex: 2024-01-01)


plans (Collection)
- Id (Ex: b6a049375f6f4445a4a884b02eedeac0)
- HabitId (Ex: cd383069fbd840b3ad503447cf9e488d)
- Description (Ex: Workout 5 times a week)
- StartsAt (Ex: 2024-01-01)
- EndsAt (Ex: 2024-12-31)
- Frequency: (Ex: { Count: 5, Per: "WEEK" })
- RestsPerPeriod (Ex: 2)
- PenaltyValueCents (Ex: 500)
- Currency (Ex: BRL)
- AdminFeePercent (Ex: 10)
- Status (NotStarted,Running,Finished,Cancelled)
- Type (Public,Private)
- CreatedAt (Ex: 2024-01-01)
- Totals (Updated with Change Stream)
  - Members (Ex: 10)
  - ComplianceCount (Ex: 45, Number of Checkins)
  - PenaltiesCount (Ex: 180)
  - PenaltiesValueCents (Ex: 90000)
  - RewardsPoolCents (Ex: 81000 // penalties - adminFee)


plan_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
- RestsAvailable (Number of rests left for the current period)


checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Role (CheckedIn,Rest)
- Date (Ex: 2024-01-05)
- Title (Ex: Let's call it a day)
- Description (Ex: Today was rough)
- Evidence (Image Link)
- ValidationFlags (List of ValidationFlag entity)
- Status (Pending,Valid,Invalid)


penalties (Collection)
- Id (Ex: 426881107ed44fca8c58d57646ae1a90)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- OccurredAt (Ex: 2024-01-10)
- DueAt (Ex: 2024-01-11)
- PaidAt (Ex: 2024-01-11)
- Status (Pending/OverDue/Paid)
- ValueCents (Ex: 500)


rewards (Collection)
- Id (Ex: fa136a773bc24920a8bc84bc0f06396c)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- ValueCents (Ex: 1000)
- Status (Pending/OverDue/Paid)
- PaidDate (Ex: 2025-01-01)


ValidationFlag (Entity)
- UserId (Ex: 4a36525e16914dd6bd4134cbfde0dd5e)
- Decision (Valid,Invalid)
- Reason (Ex: Not at the gym)
- CreatedAt (Ex: 2024-01-06)


--> Índices essenciais

users:
- { emailLower: 1 } (unique)

plans:
- { status: 1, startsAt: 1 }
- { type: 1 }

plan_members:
- { planId: 1, userId: 1 } (unique)
- { userId: 1, status: 1 }

checkins:
- { planId: 1, userId: 1, date: 1 } (unique)
- { planId: 1, periodIndex: 1 } (ranking por período)

penalties:
- { planId: 1, userId: 1, dueAt: 1 }
- { status: 1, dueAt: 1 } (cobrança em lote)

rewards:
- { planId: 1, userId: 1 }
- { status: 1 }

plans:
- { status: 1, startsAt: 1 }
- { type: 1 } (listar públicos)


*/