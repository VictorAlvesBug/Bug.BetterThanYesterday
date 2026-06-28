using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public sealed class ReviewCheckInUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPlanMemberRepository planMemberRepository)
    : IUseCase<ReviewCheckInCommand>
{
    public async Task<IResult> HandleAsync(ReviewCheckInCommand command)
    {
        try
        {
            command.Validate();

            var checkIn = await checkInRepository.GetByIdAsync(command.CheckInId);

            if (checkIn is null)
                return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

			var reviewer = await userRepository.GetByIdAsync(command.ReviewerId);

			if (reviewer is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var checkInOwner = await userRepository.GetByIdAsync(checkIn.UserId);

			if (checkInOwner is null)
				return Result.Rejected(Messages.CheckInOwnerNotFound, RejectionType.NotFound);
            
            var plan = await planRepository.GetByIdAsync(checkIn.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var planMemberId = PlanMember.BuildId(
				checkIn.PlanId,
				command.ReviewerId
			);

			var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

			if (planMember is null)
                return Result.Rejected(Messages.UserIsNotInThePlan, RejectionType.NotFound);

            if (planMember.Status == PlanMemberStatus.Blocked)
                return Result.Rejected(Messages.MemberIsBlockedInThePlan);

            if (plan.GetStatus() != PlanStatus.Running)
                return Result.Rejected(Messages.OnlyRunningPlansCanReceiveNewCheckIns);

            if (!checkIn.IsReviewWindowOpen(plan.CheckInReviewWindowInDays))
                return Result.Rejected(Messages.CheckInReviewWindowHasAlreadyClosed);

            if (checkIn.IsReviewAlreadyMadeByUser(command.ReviewerId))
                return Result.Rejected(Messages.ReviewAlreadyMadeByUserForThisCheckIn);

            if (checkIn.IsReviewerCheckInOwner(command.ReviewerId))
                return Result.Rejected(Messages.ReviewerCannotReviewHisOwnCheckIn);

            checkIn.AddReview(Review.Create(command.ReviewerId, command.Status, command.Date));

			var activeMemberCount = (await planMemberRepository.ListByPlanIdAsync(checkIn.PlanId))
				.Count(m => m.Status == PlanMemberStatus.Active);

			checkIn.ConsolidateReviewsIntoStatus(activeMemberCount, plan.CheckInReviewWindowInDays);

            await checkInRepository.UpdateAsync(checkIn);

            return Result.Success(
                checkIn.ToModel(plan, habit, checkInOwner),
                Messages.CheckInSuccessfullyRegistered
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}