using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;

namespace Bug.BetterThanYesterday.Application.Plans.ListAllPlans;

public class ListAllPlansUseCase(IPlanRepository planRepository)
	: IUseCase<ListAllPlansCommand, IResult>
{
	public async Task<IResult> HandleAsync(ListAllPlansCommand command)
	{
		try
		{
			command.Validate();
			var plans = (await planRepository.ListAllAsync()).Select(plan => plan.ToModel());
			return Result.Success(plans);
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
