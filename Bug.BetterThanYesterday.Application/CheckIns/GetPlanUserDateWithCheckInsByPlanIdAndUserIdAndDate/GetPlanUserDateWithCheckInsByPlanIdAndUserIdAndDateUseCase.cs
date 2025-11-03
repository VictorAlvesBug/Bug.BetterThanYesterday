using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate;

public sealed class GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand>
{
    public async Task<IResult> HandleAsync(GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDateCommand command)
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

            var checkIns = await checkInRepository.ListByPlanIdAndUserIdAndDateAsync(
                command.PlanId,
                command.UserId,
                DateOnly.FromDateTime(command.Date)
            );

            if (!checkIns.Any())
                return Result.Success(
                    plan.ToPlanUserDateWithCheckInsModel(user, command.Date, []),
                    Messages.CheckInNotFound
                );

            return Result.Success(
                plan.ToPlanUserDateWithCheckInsModel(user, command.Date, checkIns.ToList()),
                Messages.CheckInsSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}