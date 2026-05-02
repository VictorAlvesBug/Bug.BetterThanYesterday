using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<UpdatePlanStatusCommand>
{
	public async Task<IResult> HandleAsync(UpdatePlanStatusCommand command)
	{
		try
		{
			command.Validate();
			
			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound);

			var newStatus = PlanStatus.FromId(command.StatusId);
			plan.ChangeStatus(newStatus);

			await planRepository.UpdateAsync(plan);
			return Result.Success(plan.ToModel(habit.ToModel()), Messages.PlanStatusSuccessfullyUpdated);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
