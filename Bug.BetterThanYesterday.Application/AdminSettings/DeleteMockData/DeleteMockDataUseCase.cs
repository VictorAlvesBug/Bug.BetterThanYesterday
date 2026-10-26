using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.AdminSettings.DeleteMockData;



public sealed class DeleteMockDataUseCase(
	ICheckInRepository checkInRepository,
	IHabitRepository habitRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IChallengeRepository challengeRepository,
	IUserRepository userRepository
)
	: IUseCase<DeleteMockDataCommand>
{
	public async Task<IResult> HandleAsync(DeleteMockDataCommand command)
	{
		try
		{
			command.Validate();

			var tasks = new List<Task>
			{
				habitRepository.DeleteAllAsync(),
				userRepository.DeleteAllAsync(),
				challengeRepository.DeleteAllAsync(),
				challengeMemberRepository.DeleteAllAsync(),
				checkInRepository.DeleteAllAsync(),
			};

			await Task.WhenAll(tasks);

			return Result.Success($"Dados mockados deletedos com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
