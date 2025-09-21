using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.Habits.GetHabitById;

public sealed class GetHabitByIdUseCase(IHabitRepository habitRepository)
	: IUseCase<GetHabitByIdCommand, IResult>
{
	public async Task<IResult> HandleAsync(GetHabitByIdCommand command)
	{
		try
		{
			command.Validate();
			var habit = await habitRepository.GetByIdAsync(command.Id);

			if (habit is null)
				return Result.Rejected("Hábito não encontrado");

			return Result.Success(habit.ToModel());
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
