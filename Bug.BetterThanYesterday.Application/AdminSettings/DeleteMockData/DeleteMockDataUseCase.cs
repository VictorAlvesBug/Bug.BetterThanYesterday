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

			foreach(var habit in MockData.MockHabits)
			{
				await habitRepository.DeleteAsync(habit);
			}

			foreach(var user in MockData.MockUsers)
			{
				await userRepository.DeleteAsync(user);
			}

			foreach(var plan in MockData.MockPlans)
			{
				await planRepository.DeleteAsync(plan);
			}

			foreach(var planMember in MockData.MockPlanMembers)
			{
				await planMemberRepository.DeleteAsync(planMember);
			}

			foreach(var checkIn in MockData.MockCheckIns)
			{
				await checkInRepository.DeleteAsync(checkIn);
			}

			return Result.Success($"Dados mockados deletedos com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
