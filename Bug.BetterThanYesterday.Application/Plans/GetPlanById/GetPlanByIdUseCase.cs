using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanById;

public class GetPlanByIdUseCase(IPlanRepository planRepository)
	: IUseCase<GetPlanByIdCommand>
{
	public async Task<IResult> HandleAsync(GetPlanByIdCommand command)
	{
		command.Validate();
		var plan = await planRepository.GetByIdAsync(command.PlanId);

		if (plan is null)
			return Result.Rejected(Messages.EnterPlanId);

		return Result.Success(plan.ToModel());
	}
}
