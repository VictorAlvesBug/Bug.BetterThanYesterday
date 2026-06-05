using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.AdminSettings.PersistMockData;



public sealed class PersistMockDataUseCase(
	ICheckInRepository checkInRepository,
	IHabitRepository habitRepository,
	IPlanMemberRepository planMemberRepository,
	IPlanRepository planRepository,
	IUserRepository userRepository
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
				planRepository.InsertJsonAsync(MockData.MockPlans),
				planMemberRepository.InsertJsonAsync(MockData.MockPlanMembers),
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
