using Bug.BetterThanYesterday.API;
using Bug.BetterThanYesterday.Application.DependencyInjection;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseConfig>(
	builder.Configuration.GetSection(nameof(DatabaseConfig)));
builder.Services.AddSingleton<IDatabaseConfig>(sp =>
	sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);

builder.Services.AddSingleton(sp =>
{
	var dbConfig = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;
	var client = (IMongoClient)new MongoClient(dbConfig.ConnectionString);
	return client.GetDatabase(dbConfig.DatabaseName);
});

//builder.Services.AddTransient<IMiddleware, ExceptionHandlingMiddleware>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*

######### Cenários de teste #########

# Testes do UseCase



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
 - Test_DeleteUserUseCase_InvalidCreatedAtFromDb_ShouldReturn422(); ---
 
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
 - Test_UpdateHabitUseCase_InvalidUserId_ShouldReturn400();
 - Test_UpdateHabitUseCase_NotFoundUserId_ShouldReturn404();
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
 - Test_DeleteHabitUseCase_InvalidCreatedAtFromDb_ShouldReturn422(); --- // Definir testes de 422 (recursos inválidos vindos do banco) de planos
 
 ## ListAllPlans()
 - Test_ListAllPlansUseCase_Valid_ShouldReturn200();
 - Test_ListAllPlansUseCase_InvalidOrMissingToken_ShouldReturn401();
 
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


plan_participants (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- PlanId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
> GetById
> ListByPlanId
> ListByUserId
> Join
> Leave
> Block

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


plan_participants (Collection)
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
  - Participants (Ex: 10)
  - ComplianceCount (Ex: 45, Number of Checkins)
  - PenaltiesCount (Ex: 180)
  - PenaltiesValueCents (Ex: 90000)
  - RewardsPoolCents (Ex: 81000 // penalties - adminFee)


plan_participants (Collection)
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

plan_participants:
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