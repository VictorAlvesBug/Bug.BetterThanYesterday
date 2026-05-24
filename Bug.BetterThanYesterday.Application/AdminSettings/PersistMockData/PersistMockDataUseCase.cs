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

			foreach(var habit in MockData.MockHabits)
			{
				await habitRepository.AddAsync(habit);
			}

			foreach(var user in MockData.MockUsers)
			{
				await userRepository.AddAsync(user);
			}

			foreach(var plan in MockData.MockPlans)
			{
				await planRepository.AddAsync(plan);
			}

			foreach(var planMember in MockData.MockPlanMembers)
			{
				await planMemberRepository.AddAsync(planMember);
			}

			foreach(var checkIn in MockData.MockCheckIns)
			{
				await checkInRepository.AddAsync(checkIn);
			}

			return Result.Success($"Dados mockados persistidos com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
