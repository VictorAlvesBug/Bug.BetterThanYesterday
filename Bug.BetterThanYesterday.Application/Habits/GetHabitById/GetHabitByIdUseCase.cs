using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public sealed class GetHabitByIdUseCase(
	IHabitRepository habitRepository,
	IPlanRepository planRepository,
	IUserRepository userRepository
)
	: IUseCase<GetHabitByIdCommand>
{
	public async Task<IResult> HandleAsync(GetHabitByIdCommand command)
	{
		try
		{
			command.Validate();

			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var plans = await planRepository.ListByHabitIdAsync(habit.Id);
			var ownerIds = plans.Select(plan => plan.OwnerId).Distinct().ToList();
			var owners = ownerIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(ownerIds);

			if (ownerIds.Count > owners.Count)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			return Result.Success(habit.ToModel(plans, owners), Messages.HabitSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
