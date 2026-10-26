using Bug.BetterThanYesterday.API;
using Bug.BetterThanYesterday.API.Configurations;
using Bug.BetterThanYesterday.Application.DependencyInjection;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.DependencyInjection;
using Bug.BetterThanYesterday.Infrastructure.Persistence;
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
    var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTIONSTRING");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "Variável de ambiente MONGODB_CONNECTIONSTRING não configurada.");
    }

    return (IMongoClient)new MongoClient(connectionString);
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
builder.Services.Configure<Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport.PlatformAdminOptions>(
	builder.Configuration.GetSection(Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport.PlatformAdminOptions.SectionName));

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

if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MONGODB_CONNECTIONSTRING")))
	await MigrateChallengeCollectionsAsync(app.Services);

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

static async Task MigrateChallengeCollectionsAsync(IServiceProvider services)
{
	using var scope = services.CreateScope();
	var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();
	var config = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
	var names = new[] { config.DatabaseName, config.TestDatabaseName }
		.Where(name => !string.IsNullOrWhiteSpace(name))
		.Distinct(StringComparer.OrdinalIgnoreCase);

	foreach (var name in names)
		await ChallengeCollectionMigration.ApplyAsync(client.GetDatabase(name));
}

app.Run();

/*

TODO - Exibir progresso, sequencia e posição no challenge-tracker apenas se o plano já estiver em andamento
TODO - Validar se checkin já foi feito por este usuário na aba challenge-settings > Check-ins
TODO - Sequência está ficando sempre zerada no challenge-tracker e no ranking
TODO - Exibir "+1" ou "+2" na sequência caso checkin ainda esteja com status de pendente

TODO - On Invitation Link Modal, fix the modal to be displayed in the center of the screen, with a close button on the top right corner
TODO - Change the color of navigation bar to the gradient color from the app theme
TODO - Fix the Side Drawer animation to be smoother

TODO - Fix the bug that auto-validate checking for yesterday, using the current date as reference but keeping createdAt as user selection
TODO - Fix my keyboardable view to be able to scroll the content properly when the keyboard is open in any screen


TODO - On Challenge Settings Screen, display the challenge details, as Description, StartsAt, EndsAt, Type, DaysOffPerWeek and PenaltyValue
TODO - On Challenge Settings Screen, display the payout pool from truth data
TODO - On Challenge Settings Screen > Ranking, display current user's name with green color
TODO - On Challenge Settings Screen > Ranking, display all users profile picture with the same size
TODO - On Challenge Settings Screen > Check-ins and Challenge Tracker Screen > Check-ins, hide divider line for each current users's checkin, because the line is not needed (they cannot review their own checkins)

TODO - On Challenge Tracker Screen, camera floating button should be displayed only if the challenge is running
TODO - On Challenge Tracker Screen, camera floating button should open 2 options: Today and Yesterday
TODO - On Challenge Tracker Screen, when the user clicks on the Today option, the create checkin screen is opened with the date set to today
TODO - On Challenge Tracker Screen, when the user clicks on the Yesterday option, the create checkin screen is opened with the date set to yesterday
TODO - On Create Checkin Screen, don't allow user to change the date if thats already a checkin (pending or validated) for this date

TODO - Create a new screen (called "My Challenges") to display the challenges created by the current user
TODO - On My Challenges Screen, allow user to change the challenge settings on a Modal screen, as Description, StartsAt, EndsAt, Type, DaysOffPerWeek and PenaltyValue (if it is not started yet) and save the changes
TODO - On My Challenges Screen, allow user to delete a challenge if it is not started yet, but with a confirmation modal
TODO - On My Challenges Screen, allow user to duplicate a challenge preselecting the field on create challenge screen, and redirection there

TODO - On Public Challenges Screen > Header, display the Invitation Link Modal if the anex icon is clicked
TODO - On Public Challenges Screen > Each Challenge Card, allow user to click on the challenge card to display the Challenge Details Modal
TODO - On Public Challenges Screen > Each Challenge Card, display simplified info. Only habit name, owner's name, challenge description (if not null), days to/since startsAt, Penalty Value Icon with Dollar Signs (1 to 3) and commitment level (1/7 to 7/7)
TODO - On Public Challenges Screen > Each Challenge Card Modal, display the Share Challenge icon on the top right corner
TODO - On Manage Challenges Screen, add a new button to display My Challenges Screen

TODO - Version Obsidian Collection

TODO - Implement to delete former images from the S3 bucket when the user updates the photo
TODO - On Create User Screen and Update User Screen, add mask validation to fields (phone number, pix key)


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
- GetChallengeById with a non-guid challengeId
- GetChallengeById with a valid guid, but not registered
- GetChallengeById with a registered challengeId
- ListChallengesByFilter without any parameter
- ListChallengesByFilter with non-guid ownerId
- ListChallengesByFilter with non-registered ownerId
- ListChallengesByFilter with registered ownerId that has no challenges
- ListChallengesByFilter with registered ownerId that has challenges
- ListChallengesByFilter with non-guid habitId
- ListChallengesByFilter with non-registered habitId
- ListChallengesByFilter with registered habitId that has no challenges
- ListChallengesByFilter with registered habitId that has challenges
- ListChallengesByFilter with an invalid numeric status
- ListChallengesByFilter with an invalid text status
- ListChallengesByFilter with an invalid numeric type
- ListChallengesByFilter with an invalid text type
- ListChallengesByFilter with all field valid
- AddChallenge with an empty body
- AddChallenge without ownerId
- AddChallenge with an non-guid ownerId
- AddChallenge with an non-registered ownerId
- AddChallenge with an registered ownerId
- AddChallenge without habitId
- AddChallenge with an non-guid habitId
- AddChallenge with an non-registered habitId
- AddChallenge with an registered habitId
- AddChallenge without a description
- AddChallenge with a description
- AddChallenge without startsAt
- AddChallenge with startsAt before today
- AddChallenge without endsAt
- AddChallenge with endsAt before startsAt
- AddChallenge with endsAt before today
- AddChallenge without type
- AddChallenge with invalid numeric type
- AddChallenge with invalid text type
- AddChallenge with all field valid
- GetChallengeMemberDetails with a non-guid challengeId
- GetChallengeMemberDetails with a non-registered challengeId
- GetChallengeMemberDetails with a registered, but without members challengeId
- GetChallengeMemberDetails with a non-guid userId
- GetChallengeMemberDetails with a non-registered userId
- GetChallengeMemberDetails with a registered, but without challenges userId
- GetChallengeMemberDetails with registered challengeId and userId related
- GetChallengeWithMembersByChallengeId with a non-guid challengeId
- GetChallengeWithMembersByChallengeId with a non-registered challengeId
- GetChallengeWithMembersByChallengeId with a registered, but without members challengeId
- GetChallengeWithMembersByChallengeId with a registered, and with members challengeId
- GetUserWithChallengesByUserId with a non-guid userId
- GetUserWithChallengesByUserId with a non-registered userId
- GetUserWithChallengesByUserId with a registered, but without challenges userId
- GetUserWithChallengesByUserId with a registered, and with challenges userId
- AddUserToChallenge with a non-guid challengeId
- AddUserToChallenge with a non-registered challengeId
- AddUserToChallenge with a non-guid userId
- AddUserToChallenge with a non-registered userId
- AddUserToChallenge with registered challengeId and userId, not related yet
- AddUserToChallenge with registered challengeId and userId, already related
- AddUserToChallenge with registered challengeId and userId, and not-started challenge
- AddUserToChallenge with registered challengeId and userId, and running challenge
- AddUserToChallenge with registered challengeId and userId, and finished challenge
- AddUserToChallenge with registered challengeId and userId, and cancelled challenge
- RemoveUserFromChallenge with a non-guid challengeId
- RemoveUserFromChallenge with a non-registered challengeId
- RemoveUserFromChallenge with a non-guid userId
- RemoveUserFromChallenge with a non-registered userId
- RemoveUserFromChallenge with registered challengeId and userId, not related
- RemoveUserFromChallenge with registered challengeId and userId related and active
- RemoveUserFromChallenge with registered challengeId and userId related and blocked
- RemoveUserFromChallenge with registered challengeId and userId, and not-started challenge
- RemoveUserFromChallenge with registered challengeId and userId, and running challenge
- RemoveUserFromChallenge with registered challengeId and userId, and finished challenge
- RemoveUserFromChallenge with registered challengeId and userId, and cancelled challenge
- BlockUserInTheChallenge with a non-guid challengeId
- BlockUserInTheChallenge with a non-registered challengeId
- BlockUserInTheChallenge with a non-guid userId
- BlockUserInTheChallenge with a non-registered userId
- BlockUserInTheChallenge with registered challengeId and userId, not related
- BlockUserInTheChallenge with registered challengeId and userId related and active
- BlockUserInTheChallenge with registered challengeId and userId related and blocked
- BlockUserInTheChallenge with registered challengeId and userId, and not-started challenge
- BlockUserInTheChallenge with registered challengeId and userId, and running challenge
- BlockUserInTheChallenge with registered challengeId and userId, and finished challenge
- BlockUserInTheChallenge with registered challengeId and userId, and cancelled challenge
- UnblockUserInTheChallenge with a non-guid challengeId
- UnblockUserInTheChallenge with a non-registered challengeId
- UnblockUserInTheChallenge with a non-guid userId
- UnblockUserInTheChallenge with a non-registered userId
- UnblockUserInTheChallenge with registered challengeId and userId, not related
- UnblockUserInTheChallenge with registered challengeId and userId related and active
- UnblockUserInTheChallenge with registered challengeId and userId related and blocked
- UnblockUserInTheChallenge with registered challengeId and userId, and not-started challenge
- UnblockUserInTheChallenge with registered challengeId and userId, and running challenge
- UnblockUserInTheChallenge with registered challengeId and userId, and finished challenge
- UnblockUserInTheChallenge with registered challengeId and userId, and cancelled challenge
- GetCheckInById with a non-guid checkIn
- GetCheckInById with a valid guid, but not registered
- GetCheckInById with a registered checkIn
- ListCheckInsByFilter with a non-guid challengeId
- ListCheckInsByFilter with a non-registered challengeId
- ListCheckInsByFilter with a registered challengeId, without any checkIns related
- ListCheckInsByFilter with a registered challengeId, with checkIns related
- ListCheckInsByFilter only with userId
- ListCheckInsByFilter only with date
- ListCheckInsByFilter only with status
- ListCheckInsByFilter with a registered challengeId and non-guid userId
- ListCheckInsByFilter with a registered challengeId and non-registered userId
- ListCheckInsByFilter with a registered challengeId and registered userId, but not related
- ListCheckInsByFilter with a registered challengeId and registered userId, related via challengeMembers, but with no checkIns
- ListCheckInsByFilter with a registered challengeId and registered userId, related and with some checkIns
- ListCheckInsByFilter with a registered challengeId and date
- ListCheckInsByFilter with a registered challengeId and invalid numeric status
- ListCheckInsByFilter with a registered challengeId and invalid text status
- ListCheckInsByFilter with a registered challengeId and valid numberic status
- ListCheckInsByFilter with a registered challengeId and valid text status
- AddCheckIn with empty body
- AddCheckIn without challengeId
- AddCheckIn with non-guid challengeId
- AddCheckIn with non-registered challengeId
- AddCheckIn with registered challengeId
- AddCheckIn without userId
- AddCheckIn with non-guid userId
- AddCheckIn with non-registered userId
- AddCheckIn with registered userId
- AddCheckIn without title
- AddCheckIn with white space title
- AddCheckIn without evidencePhotoUrl
- AddCheckIn with empty evidencePhotoUrl
- AddCheckIn without date
- AddCheckIn with challengeId and userId not related via challengeMembers
- AddCheckIn with user blocked in the challenge
- AddCheckIn with date before yesterday
- AddCheckIn with date after today
- AddCheckIn with date before challenge startsAt
- AddCheckIn with date after challenge endsAt
- AddCheckIn with not-started challenge
- AddCheckIn with running challenge
- AddCheckIn with finished challenge
- AddCheckIn with cancelled challenge
- AddCheckIn with same fields already registered



- ReviewCheckIn with empty body
- ReviewCheckIn without checkInId
- ReviewCheckIn with non-guid checkInId
- ReviewCheckIn with non-registered checkInId
- ReviewCheckIn with registered checkInId
- ReviewCheckIn without reviewerId
- ReviewCheckIn with non-guid reviewerId
- ReviewCheckIn with non-registered reviewerId
- ReviewCheckIn with registered reviewerId, not related with the challenge
- ReviewCheckIn with registered reviewerId, related with the challenge
- ReviewCheckIn with registered reviewerId, to his own checkIn
- ReviewCheckIn without status
- ReviewCheckIn with invalid numeric status
- ReviewCheckIn with invalid text status
- ReviewCheckIn with valid numeric status
- ReviewCheckIn with valid text status
- ReviewCheckIn without date
- ReviewCheckIn with date before checkIn createdAt
- ReviewCheckIn with date after checkIn date+1
- ReviewCheckIn for checkIn from a not-started challenge
- ReviewCheckIn for checkIn from a running challenge
- ReviewCheckIn for checkIn from a finished challenge
- ReviewCheckIn for checkIn from a cancelled challenge
- ReviewCheckIn for checkIn already reviewed by this reviewer
- ReviewCheckIn for a non-pending checkIn

######### Cen?rios de teste #########

# Testes do UseCase

## CheckIn

- CheckInId (ChallengeId,UserId,Date,Index=0)
- ChallengeId
- UserId
- Date
- Index
- Title
- Description

--> GetCheckInDetailsUseCase
(challengeId,userId,date,index=0)
- Valid
- NotFoundCheckInId


--> GetChallengeWithCheckInsByChallengeId
(challengeId)
- Valid
- ChallengeWithoutCheckIns
- NotFoundChallengeId


--> GetChallengeUserWithCheckInsByChallengeIdAndUserId
(challengeId,userId)
- Valid
- ChallengeUserWithoutCheckIns
- UserIsNotInThisChallenge
- NotFoundChallengeId
- NotFoundUserId


--> AddCheckIn
(challengeId,userId,date,index=0,title,description)
- Valid
- NotFoundChallengeId
- NotFoundUserId
- DateOutOfAllowedRange
- IndexOutOfAllowedRange
- DuplicatedCheckIn
- UserIsNotInThisChallenge
- ChallengeIsNotRunning
-?UserIsBlocked

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
 - Test_DeleteHabitUseCase_NonCancelledChallengesRelated_ShouldReturn400();
 - Test_DeleteHabitUseCase_EmptyNameFromDb_ShouldReturn422();
 - Test_DeleteHabitUseCase_EmptyCreatedAtFromDb_ShouldReturn422();
 - Test_DeleteHabitUseCase_InvalidCreatedAtFromDb_ShouldReturn422();

---

// Definir testes de 422 (recursos inv?lidos vindos do banco) de planos
 
 ## ListChallengesByFilter()
 - Test_ListChallengesByFilterUseCase_Valid_ShouldReturn200();
 - Test_ListChallengesByFilterUseCase_InvalidOrMissingToken_ShouldReturn401();
 
 ## GetChallengeById(id)
 - Test_GetChallengeByIdUseCase_Valid_ShouldReturn200();
 - Test_GetChallengeByIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_GetChallengeByIdUseCase_InvalidChallengeId_ShouldReturn400();
 - Test_GetChallengeByIdUseCase_NotFoundChallengeId_ShouldReturn404();
 
 ## ListChallengesByHabitId(habitId)
 - Test_ListChallengesByHabitIdUseCase_Valid_ShouldReturn200();
 - Test_ListChallengesByHabitIdUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_ListChallengesByHabitIdUseCase_InvalidHabitId_ShouldReturn400();
 - Test_ListChallengesByHabitIdUseCase_NotFoundHabitId_ShouldReturn404();
 
 ## CreateChallenge(habitId, description, startsAt, endsAt, typeId)
 - Test_CreateChallengeUseCase_Valid_ShouldReturn201();
 - Test_CreateChallengeUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_CreateChallengeUseCase_InvalidHabitId_ShouldReturn400();
 - Test_CreateChallengeUseCase_NotFoundHabitId_ShouldReturn404();
 - Test_CreateChallengeUseCase_EmptyStartsAt_ShouldReturn400();
 - Test_CreateChallengeUseCase_StartsAtBeforeToday_ShouldReturn400();
 - Test_CreateChallengeUseCase_EmptyEndsAt_ShouldReturn400();
 - Test_CreateChallengeUseCase_EndsAtBeforeStartsAt_ShouldReturn400();
 - Test_CreateChallengeUseCase_EmptyTypeId_ShouldReturn400();
 - Test_CreateChallengeUseCase_InvalidTypeId_ShouldReturn400();
 
 ## UpdateChallengeStatus(id, statusId)
 - Test_UpdateChallengeStatusUseCase_Valid_ShouldReturn200();
 - Test_UpdateChallengeStatusUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_UpdateChallengeStatusUseCase_InvalidChallengeId_ShouldReturn400();
 - Test_UpdateChallengeStatusUseCase_NotFoundChallengeId_ShouldReturn404();
 - Test_UpdateChallengeStatusUseCase_EmptyStatusId_ShouldReturn400();
 - Test_UpdateChallengeStatusUseCase_InvalidStatusId_ShouldReturn400();
 - Test_UpdateChallengeStatusUseCase_StatusIdNotAllowed_ShouldReturn400();
 
 ## CancelChallenge(id)
 - Test_CancelChallengeUseCase_Valid_ShouldReturn200();
 - Test_CancelChallengeUseCase_InvalidOrMissingToken_ShouldReturn401();
 - Test_CancelChallengeUseCase_InvalidChallengeId_ShouldReturn400();
 - Test_CancelChallengeUseCase_NotFoundChallengeId_ShouldReturn404();



########################################  Vers?o 1  ########################################

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


challenges (Collection)
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


challenge_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
> GetById
> ListByChallengeId
> ListByUserId
> AddUserToChallenge
> RemoveUserFromChallenge
> BlockUserInTheChallenge
> UnblockUserInTheChallenge

checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Date (Ex: 2024-01-05)
- Title (Ex: Let's call it a day)
- Description (Ex: Today was rough)
> GetById
> ListByChallengeId
> ListByChallengeIdAndUserId
> Check


########################################  Vers?o 2  ########################################

users (Collection)
- Id (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Name (Ex: John Doe)
- Email (Ex: johndoe@gmail.com) (unique)
- CreatedAt (Ex: 2024-01-01)


habits (Collection)
- Id (Ex: cd383069fbd840b3ad503447cf9e488d)
- Name (Ex: Workout)
- CreatedAt (Ex: 2024-01-01)


challenges (Collection)
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


challenge_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
- RestsAvailable (Number of rests left for the current period)


checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- Role (CheckedIn,Rest)
- Date (Ex: 2024-01-05)
- Title (Ex: Let's call it a day)
- Description (Ex: Today was rough)
- Evidence (Image Link)


########################################  Vers?o 3  ########################################

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


challenges (Collection)
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


challenge_members (Collection)
- Id (Ex: d1f8e8c2a4b14c3e9f0e4b5a6c7d8e9f)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- JoinedAt (Ex: 2024-01-15)
- LeftAt (Nullable, Ex: 2024-06-01)
- Status (Active,Left,Blocked)
- RestsAvailable (Number of rests left for the current period)


checkins (Collection)
- Id (Ex: f1988915ee294c34bdc6ff8b3c467cdc)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
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
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- OccurredAt (Ex: 2024-01-10)
- DueAt (Ex: 2024-01-11)
- PaidAt (Ex: 2024-01-11)
- Status (Pending/OverDue/Paid)
- ValueCents (Ex: 500)


rewards (Collection)
- Id (Ex: fa136a773bc24920a8bc84bc0f06396c)
- ChallengeId (Ex: b6a049375f6f4445a4a884b02eedeac0)
- UserId (Ex: 450e04cff74e4fbc86e7f100a13acd4b)
- ValueCents (Ex: 1000)
- Status (Pending/OverDue/Paid)
- PaidDate (Ex: 2025-01-01)


ValidationFlag (Entity)
- UserId (Ex: 4a36525e16914dd6bd4134cbfde0dd5e)
- Decision (Valid,Invalid)
- Reason (Ex: Not at the gym)
- CreatedAt (Ex: 2024-01-06)


--> ?ndices essenciais

users:
- { emailLower: 1 } (unique)

challenges:
- { status: 1, startsAt: 1 }
- { type: 1 }

challenge_members:
- { challengeId: 1, userId: 1 } (unique)
- { userId: 1, status: 1 }

checkins:
- { challengeId: 1, userId: 1, date: 1 } (unique)
- { challengeId: 1, periodIndex: 1 } (ranking por per?odo)

penalties:
- { challengeId: 1, userId: 1, dueAt: 1 }
- { status: 1, dueAt: 1 } (cobran?a em lote)

rewards:
- { challengeId: 1, userId: 1 }
- { status: 1 }

challenges:
- { status: 1, startsAt: 1 }
- { type: 1 } (listar p?blicos)


*/