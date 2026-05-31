using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.AdminSettings.DeleteMockData;



public sealed class DeleteMockDataUseCase(
	ICheckInRepository checkInRepository,
	IHabitRepository habitRepository,
	IPlanMemberRepository planMemberRepository,
	IPlanRepository planRepository,
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
				habitRepository.DeleteManyAsync(MockData.MockHabits),
				userRepository.DeleteManyAsync(MockData.MockUsers),
				planRepository.DeleteManyAsync(MockData.MockPlans),
				planMemberRepository.DeleteManyAsync(MockData.MockPlanMembers),
				checkInRepository.DeleteManyAsync(MockData.MockCheckIns),
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
