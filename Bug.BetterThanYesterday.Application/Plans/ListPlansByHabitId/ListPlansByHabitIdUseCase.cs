using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;

public class ListPlansByHabitIdUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<ListPlansByHabitIdCommand>
{
	public async Task<IResult> HandleAsync(ListPlansByHabitIdCommand command)
	{
		try
		{
			command.Validate();
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var plans = await planRepository.ListByHabitIdAsync(command.HabitId);
			var ownerIds = plans.Select(plan => plan.OwnerId).Distinct().ToList();
			var owners = ownerIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(ownerIds);

			if (ownerIds.Count > owners.Count)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var ownersById = owners.ToDictionary(owner => owner.Id);
			var planModels = plans.Select(plan => plan.ToModel(habit, ownersById[plan.OwnerId]));

			return Result.Success(
				planModels,
				Messages.PlansSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
