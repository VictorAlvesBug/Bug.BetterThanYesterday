using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.ListAllPlans;

public class ListAllPlansUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<ListAllPlansCommand>
{
	public async Task<IResult> HandleAsync(ListAllPlansCommand command)
	{
		try
		{
			command.Validate();
			
			var plans = await planRepository.ListAllAsync();

			var tasks = plans.Select(async plan =>
			{
				var habit = await habitRepository.GetByIdAsync(plan.HabitId) ?? throw new Exception(Messages.HabitNotFound);
                return plan.ToModel(habit.ToModel());
			}).ToList();
			
			return Result.Success(
				await Task.WhenAll(tasks),
				Messages.PlansSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
