using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Plans.CancelPlan;

public class CancelPlanUseCase(IPlanRepository planRepository)
	: IUseCase<CancelPlanCommand, IResult>
{
	public async Task<IResult> HandleAsync(CancelPlanCommand command)
	{
		try
		{
			command.Validate();
			var plan = await planRepository.GetByIdAsync(command.Id);

			if (plan is null)
				return Result.Rejected("Plano não encontrado");

			plan.ChangeStatus(PlanStatus.Cancelled);

			await planRepository.UpdateAsync(plan);
			return Result.Success(plan.ToModel(), "Plano cancelado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
