using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanUseCase(IPlanRepository planRepository)
	: IUseCase<CancelPlanCommand>
{
	public async Task<IResult> HandleAsync(CancelPlanCommand command)
	{
		command.Validate();
		var plan = await planRepository.GetByIdAsync(command.PlanId);

		if (plan is null)
			return Result.Rejected("Plano não encontrado");

		plan.ChangeStatus(PlanStatus.Cancelled);

		await planRepository.UpdateAsync(plan);
		return Result.Success(plan.ToModel(), "Plano cancelado com sucesso");
	}
}
