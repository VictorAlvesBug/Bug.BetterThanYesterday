using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanStatus;

public class UpdatePlanStatusUseCase(IPlanRepository planRepository)
	: IUseCase<UpdatePlanStatusCommand>
{
	public async Task<IResult> HandleAsync(UpdatePlanStatusCommand command)
	{
		command.Validate();
		var plan = await planRepository.GetByIdAsync(command.PlanId);

		if (plan is null)
			return Result.Rejected("Plano não encontrado");

		var newStatus = PlanStatus.FromId(command.StatusId);
		plan.ChangeStatus(newStatus);

		await planRepository.UpdateAsync(plan);
		return Result.Success(plan.ToModel(), "Status do plano atualizado com sucesso");
	}
}
