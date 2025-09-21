using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanById;

public class GetPlanByIdUseCase(IPlanRepository planRepository)
	: IUseCase<GetPlanByIdCommand, IResult>
{
	public async Task<IResult> HandleAsync(GetPlanByIdCommand command)
	{
		try
		{
			command.Validate();
			var plan = await planRepository.GetByIdAsync(command.Id);

			if (plan is null)
				return Result.Rejected("Plano não encontrado");

			return Result.Success(plan.ToModel());
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
