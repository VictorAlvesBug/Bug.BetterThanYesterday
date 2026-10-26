using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public sealed class ReviewCheckInUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeCycleNotificationProcessor notificationProcessor)
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
            
            var challenge = await challengeRepository.GetByIdAsync(checkIn.ChallengeId);

            if (challenge is null)
                return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

            var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

            if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var challengeMemberId = ChallengeMember.BuildId(
				checkIn.ChallengeId,
				command.ReviewerId
			);

			var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

			if (challengeMember is null)
                return Result.Rejected(Messages.UserIsNotInTheChallenge, RejectionType.NotFound);

            if (challengeMember.Status == ChallengeMemberStatus.Blocked)
                return Result.Rejected(Messages.MemberIsBlockedInTheChallenge);

            if (challenge.GetStatus() != ChallengeStatus.Running)
                return Result.Rejected(Messages.OnlyRunningChallengesCanReceiveNewCheckIns);

            if (!checkIn.IsReviewWindowOpen())
                return Result.Rejected(Messages.CheckInReviewWindowHasAlreadyClosed);

            if (checkIn.IsReviewAlreadyMadeByUser(command.ReviewerId))
                return Result.Rejected(Messages.ReviewAlreadyMadeByUserForThisCheckIn);

            if (checkIn.IsReviewerCheckInOwner(command.ReviewerId))
                return Result.Rejected(Messages.ReviewerCannotReviewHisOwnCheckIn);

            checkIn.AddReview(Review.Create(command.ReviewerId, command.Status, command.Date));

            await checkInRepository.UpdateAsync(checkIn);

            try
            {
                var activeReviews = checkIn.Reviews.Where(review => review.Enable).ToList();
                var rejections = activeReviews.Count(review => review.Status == CheckInStatus.Rejected);
                var atRisk = checkIn.IsReviewWindowOpen()
                    && activeReviews.Count > 0
                    && rejections > activeReviews.Count / 2;

                await notificationProcessor.NotifyCheckInReviewedAsync(
                    challenge.Id,
                    checkIn.Id,
                    checkIn.UserId,
                    reviewer.Id,
                    reviewer.Nickname,
                    command.Status == CheckInStatus.Rejected.Name,
                    atRisk);
            }
            catch
            {
                // O aviso não impede a avaliação.
            }

            return Result.Success(
                checkIn.ToModel(challenge, habit, checkInOwner),
                Messages.CheckInSuccessfullyRegistered
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}