using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.ChallengeMembers;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.RemoveUserFromChallenge;

public sealed class RemoveUserFromChallengeUseCase(
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IChallengeFinancesProcessor challengeFinancesProcessor,
    IChallengeCycleNotificationProcessor challengeCycleNotificationProcessor)
    : IUseCase<RemoveUserFromChallengeCommand>
{
    public async Task<IResult> HandleAsync(RemoveUserFromChallengeCommand command)
    {
        try
        {
            command.Validate();
            
            var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

            if (challenge is null)
                return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

            var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var owner = challenge.OwnerId == user.Id
                ? user
                : (await userRepository.BatchGetByIdAsync([challenge.OwnerId])).SingleOrDefault();

            if (owner is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var challengeMemberId = ChallengeMember.BuildId(command.ChallengeId, command.UserId);

            var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

            if (challengeMember is null)
                return Result.Rejected(Messages.UserIsNotInTheChallenge, RejectionType.NotFound);

            var allowedChallengeStatuses = new List<ChallengeStatus> { ChallengeStatus.NotStarted, ChallengeStatus.Running };

            if (!allowedChallengeStatuses.Contains(challenge.GetStatus()))
                return Result.Rejected(Messages.MemberCanOnlyBeRemovedFromNotStartedOrRunningChallenges);

            await challengeMemberRepository.DeleteAsync(challengeMember);

            var remainingMembers = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);

            if (challenge.GetStatus() == ChallengeStatus.Running && remainingMembers.Count < 2)
            {
                challenge.Cancel();
                await challengeRepository.UpdateAsync(challenge);
                await challengeFinancesProcessor.ProcessAsync(challenge.Id);

                var recipients = remainingMembers
                    .Select(member => member.UserId)
                    .Append(command.UserId)
                    .Distinct()
                    .ToList();
                await challengeCycleNotificationProcessor.NotifyChallengeCancelledForQuorumAsync(challenge.Id, recipients);

                foreach (var member in remainingMembers)
                    await challengeMemberRepository.DeleteAsync(member);
            }

            return Result.Success(
                challengeMember.ToChallengeMemberDetailsModel(habit, challenge, owner, user),
                Messages.MemberSuccessfullyRemovedFromTheChallenge
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}