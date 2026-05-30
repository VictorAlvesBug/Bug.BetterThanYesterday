using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<CancelPlanCommand>
{
	public async Task<IResult> HandleAsync(CancelPlanCommand command)
	{
		try
		{
			command.Validate();
		
			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var owner = await userRepository.GetByIdAsync(plan.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			plan.Cancel();

			await planRepository.UpdateAsync(plan);
			return Result.Success(plan.ToModel(habit, owner), Messages.PlanSuccessfullyCancelled);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
