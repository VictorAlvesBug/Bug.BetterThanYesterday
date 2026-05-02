using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanById;

public class GetPlanByIdUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<GetPlanByIdCommand>
{
	public async Task<IResult> HandleAsync(GetPlanByIdCommand command)
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

			return Result.Success(
				plan.ToModel(habit.ToModel()),
				Messages.PlanSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
