using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserWithCheckInsByPlanIdAndUserId;

public sealed class GetPlanUserWithCheckInsByPlanIdAndUserIdUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanUserWithCheckInsByPlanIdAndUserIdCommand>
{
    public async Task<IResult> HandleAsync(GetPlanUserWithCheckInsByPlanIdAndUserIdCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);
            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);
            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            var checkIns = await checkInRepository.ListByPlanIdAndUserIdAsync(
                command.PlanId,
                command.UserId
            );

            if (!checkIns.Any())
                return Result.Success(
                    plan.ToPlanUserWithCheckInsModel(user, []),
                    Messages.CheckInNotFound
                );

            return Result.Success(
                plan.ToPlanUserWithCheckInsModel(user, checkIns.ToList()),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}