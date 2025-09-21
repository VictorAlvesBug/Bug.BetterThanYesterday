using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<CreatePlanCommand, IResult>
{
	public async Task<IResult> HandleAsync(CreatePlanCommand command)
	{
		try
		{
			command.Validate();
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected("Hábito não encontrado");

			var plan = Plan.CreateNew(
				command.HabitId,
				command.Description,
				command.StartsAt,
				command.EndsAt,
				command.TypeId);

			await planRepository.AddAsync(plan);
			return Result.Success(plan.ToModel(), "Plano cadastrado com sucesso.");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
