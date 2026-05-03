using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public sealed class GetHabitByIdUseCase(
	IHabitRepository habitRepository,
	IPlanRepository planRepository
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
				return Result.Rejected(Messages.HabitNotFound);

			var plans = await planRepository.ListByHabitIdAsync(habit.Id);

			return Result.Success(habit.ToModel(plans), Messages.HabitSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
