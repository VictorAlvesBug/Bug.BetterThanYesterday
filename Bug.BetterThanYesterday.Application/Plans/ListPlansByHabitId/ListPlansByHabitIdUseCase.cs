using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;

namespace Bug.BetterThanYesterday.Application.Plans.ListPlansByHabitId;

public class ListPlansByHabitIdUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<ListPlansByHabitIdCommand, IResult>
{
	public async Task<IResult> HandleAsync(ListPlansByHabitIdCommand command)
	{
		try
		{
			command.Validate();
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected("Hábito não encontrado");

			var plans = (await planRepository.ListByHabitIdAsync(command.HabitId))
				.Select(habit => habit.ToModel());

			return Result.Success(plans);
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
