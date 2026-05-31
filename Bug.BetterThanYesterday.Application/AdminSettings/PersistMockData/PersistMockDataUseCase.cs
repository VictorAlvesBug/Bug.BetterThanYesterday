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

			var tasks = new List<Task>
			{
				Parallel.ForEachAsync(
					MockData.MockHabits,
					async (habit, _) => await habitRepository.ReplaceAsync(habit)
				),
				Parallel.ForEachAsync(
					MockData.MockUsers,
					async (user, _) => await userRepository.ReplaceAsync(user)
				),
				Parallel.ForEachAsync(
					MockData.MockPlans,
					async (plan, _) => await planRepository.ReplaceAsync(plan)
				),
				Parallel.ForEachAsync(
					MockData.MockPlanMembers,
					async (planMember, _) => await planMemberRepository.ReplaceAsync(planMember)
				),
				Parallel.ForEachAsync(
					MockData.MockCheckIns,
					async (checkIn, _) => await checkInRepository.ReplaceAsync(checkIn)
				),
			};

			await Task.WhenAll(tasks);

			return Result.Success($"Dados mockados persistidos com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
