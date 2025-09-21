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