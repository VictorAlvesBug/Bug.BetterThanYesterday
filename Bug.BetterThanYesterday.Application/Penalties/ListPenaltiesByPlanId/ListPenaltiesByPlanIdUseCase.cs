using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanId;

public sealed class ListPenaltiesByPlanIdUseCase(
	IPenaltyRepository penaltyRepository,
	IPlanRepository planRepository)
	: IUseCase<ListPenaltiesByPlanIdCommand>
{
	public async Task<IResult> HandleAsync(ListPenaltiesByPlanIdCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var penalties = await penaltyRepository.ListByPlanIdAsync(command.PlanId);

			return Result.Success(
				penalties.Select(PenaltyMapper.ToModel).ToList(),
				Messages.PenaltiesSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
