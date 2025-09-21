using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusUseCase(IPlanRepository planRepository)
	: IUseCase<UpdatePlanStatusCommand, IResult>
{
	public async Task<IResult> HandleAsync(UpdatePlanStatusCommand command)
	{
		try
		{
			command.Validate();
			var plan = await planRepository.GetByIdAsync(command.Id);

			if (plan is null)
				return Result.Rejected("Plano não encontrado");

			plan.ChangeStatus(PlanStatus.FromId(command.Status));

			await planRepository.UpdateAsync(plan);
			return Result.Success(plan.ToModel(), "Status do plano atualizado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
