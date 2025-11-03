using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanWithCheckInsByPlanId;

public sealed class GetPlanWithCheckInsByPlanIdUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository)
    : IUseCase<GetPlanWithCheckInsByPlanIdCommand>
{
    public async Task<IResult> HandleAsync(GetPlanWithCheckInsByPlanIdCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);
            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var checkIns = await checkInRepository.ListByPlanIdAsync(command.PlanId);

            if (!checkIns.Any())
                return Result.Success(
                    plan.ToPlanWithCheckInsModel([]),
                    Messages.CheckInNotFound
                );

            return Result.Success(
                plan.ToPlanWithCheckInsModel(checkIns.ToList()),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}