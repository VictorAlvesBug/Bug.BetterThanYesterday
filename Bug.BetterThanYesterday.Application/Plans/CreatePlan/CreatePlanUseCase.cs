using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<CreatePlanCommand>
{
	public async Task<IResult> HandleAsync(CreatePlanCommand command)
	{
		try
		{
			command.Validate();
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound);

			var plan = Plan.CreateNew(
				command.HabitId,
				command.Description,
				command.StartsAt,
				command.EndsAt,
				command.TypeId);

			await planRepository.AddAsync(plan);
			return Result.Success(plan.ToModel(), Messages.PlanSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
