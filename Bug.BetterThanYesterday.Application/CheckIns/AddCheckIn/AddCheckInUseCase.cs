using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
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
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPlanMemberRepository planMemberRepository,
    IPlanCycleNotificationProcessor notificationProcessor)
    : IUseCase<AddCheckInCommand>
{
    public async Task<IResult> HandleAsync(AddCheckInCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var planMemberId = PlanMember.BuildId(
                command.PlanId,
                command.UserId
            );
            var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

            if (planMember is null)
                return Result.Rejected(Messages.PlanMemberNotFound, RejectionType.NotFound);

            var checkIns = await checkInRepository.ListByPlanIdAndUserIdAndDateAsync(
                command.PlanId,
                command.UserId,
                DateOnly.FromDateTime(command.Date)
            );

            var nextIndex = checkIns.Any()
                ? checkIns.Max(ci => ci.Index) + 1
                : 1;

            var maxIndexPerDateAllowed = plan.GetMaxCheckInsPerDateAllowed();

            if (planMember.Status != PlanMemberStatus.Active)
                return Result.Rejected(Messages.OnlyActiveMembersCanMakeCheckIns);

            if (plan.GetStatus() != PlanStatus.Running)
                return Result.Rejected(Messages.OnlyRunningPlansCanReceiveNewCheckIns);

            var members = await planMemberRepository.ListByPlanIdAsync(plan.Id);

            if (members.Count < 2)
                return Result.Rejected(Messages.PlanNeedsAtLeastTwoMembers);

            var checkInDay = DateOnly.FromDateTime(command.Date);
            var today = DateOnly.FromDateTime(DateTime.Today);
            var yesterday = today.AddDays(-1);

            if (checkInDay != today && checkInDay != yesterday)
                return Result.Rejected(Messages.CheckInDateMustBeTodayOrYesterday);

            if (command.Date < plan.StartsAt.ToDateTime(TimeOnly.MinValue) || command.Date > plan.EndsAt.ToDateTime(TimeOnly.MaxValue))
                return Result.Rejected(Messages.OnlyRunningPlansCanReceiveNewCheckIns);

            if (nextIndex > maxIndexPerDateAllowed)
                return Result.Rejected(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay);
                
            var checkIn = CheckIn.CreateNew(
                command.PlanId,
                command.UserId,
                command.Date,
                nextIndex,
                command.Title,
                command.EvidencePhotoUrl);

            await checkInRepository.AddAsync(checkIn);

            try
            {
                await notificationProcessor.NotifyCheckInAsync(plan.Id, user.Id, user.Name, members);
            }
            catch
            {
                // O aviso não impede o check-in.
            }

            return Result.Success(
                checkIn.ToModel(plan, habit, user),
                Messages.CheckInSuccessfullyRegistered
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}