using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public sealed class AddCheckInUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository,
    IPlanMemberRepository planMemberRepository)
    : IUseCase<AddCheckInCommand>
{
    public async Task<IResult> HandleAsync(AddCheckInCommand command)
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

            var planMemberId = PlanMember.BuildId(
                command.PlanId,
                command.UserId
            );
            var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

            if (planMember is null)
                return Result.Rejected(Messages.PlanMemberNotFound);

            var checkIns = await checkInRepository.ListByPlanIdAndUserIdAndDateAsync(
                command.PlanId,
                command.UserId,
                DateOnly.FromDateTime(command.Date)
            );

            var nextIndex = checkIns.Any()
                ? checkIns.Max(ci => ci.Index) + 1
                : 1;

            var maxIndexPerDateAllowed = plan.GetMaxCheckInsPerDateAllowed();

            List<PlanMemberStatus> allowedPlanMemberStatuses = [
                PlanMemberStatus.Active,
                PlanMemberStatus.Blocked
            ];

            if (!allowedPlanMemberStatuses.Contains(planMember.Status))
                return Result.Rejected(Messages.OnlyActiveMembersCanMakeCheckIns);

            if (plan.Status != PlanStatus.Running)
                return Result.Rejected(Messages.OnlyRunningPlansCanReceiveNewCheckIns);

            if (nextIndex > maxIndexPerDateAllowed)
                return Result.Rejected(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay);
                
            var checkIn = CheckIn.CreateNew(
                command.PlanId,
                command.UserId,
                command.Date,
                nextIndex,
                command.Title,
                command.Description);

            await checkInRepository.AddAsync(checkIn);

            return Result.Success(
                checkIn.ToModel(),
                Messages.CheckInSuccessfullyRegistered);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}