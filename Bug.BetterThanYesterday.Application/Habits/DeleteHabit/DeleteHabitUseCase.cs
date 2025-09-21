using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Habits.DeleteHabit;

public class DeleteHabitUseCase(
	IHabitRepository habitRepository,
	IPlanRepository planRepository)
	: IUseCase<DeleteHabitCommand, IResult>
{
	public async Task<IResult> HandleAsync(DeleteHabitCommand command)
	{
		try
		{
			command.Validate();
			var habit = await habitRepository.GetByIdAsync(command.Id);

			if (habit is null)
				return Result.Rejected("Hábito não encontrado");

			var plans = await planRepository.ListByHabitIdAsync(command.Id);

			if (plans.Any(plan => plan.Status != PlanStatus.Cancelled))
				return Result.Rejected("Hábito não pode ser removido, pois possui planos vinculados");

			await habitRepository.DeleteAsync(habit);
			return Result.Success("Hábito deletado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
