using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
//using Bug.BetterThanYesterday.Domain.Habits;
//using Bug.BetterThanYesterday.Domain.Challenges;
//using Bug.BetterThanYesterday.Domain.ChallengeMembers;
//using Bug.BetterThanYesterday.Domain.CheckIns;
//using Bug.BetterThanYesterday.Domain.Users;
//using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.AdminSettings.PersistMockData;



public sealed class PersistMockDataUseCase(
	/*ICheckInRepository checkInRepository,
	IHabitRepository habitRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IChallengeRepository challengeRepository,
	IUserRepository userRepository*/
)
	: IUseCase<PersistMockDataCommand>
{
	public async Task<IResult> HandleAsync(PersistMockDataCommand command)
	{
		try
		{
			command.Validate();

			/*var tasks = new List<Task>
			{
				habitRepository.InsertJsonAsync(MockData.MockHabits),
				userRepository.InsertJsonAsync(MockData.MockUsers),
				challengeRepository.InsertJsonAsync(MockData.MockChallenges),
				challengeMemberRepository.InsertJsonAsync(MockData.MockChallengeMembers),
				checkInRepository.InsertJsonAsync(MockData.MockCheckIns)
			};

			await Task.WhenAll(tasks);*/

			return Result.Success($"Dados mockados persistidos com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
