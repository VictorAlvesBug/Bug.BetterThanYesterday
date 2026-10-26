using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.UnblockUserInTheChallenge;

public sealed class UnblockUserInTheChallengeUseCase(
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<UnblockUserInTheChallengeCommand>
{
    public async Task<IResult> HandleAsync(UnblockUserInTheChallengeCommand command)
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

            if (challenge.GetStatus() != ChallengeStatus.Running)
                return Result.Rejected(Messages.MemberCanOnlyBeUnblockedInRunningChallenges);

            challengeMember.MarkAsActive();

            await challengeMemberRepository.UpdateAsync(challengeMember);
            return Result.Success(
                challengeMember.ToChallengeMemberDetailsModel(habit, challenge, owner, user),
                Messages.MemberSuccessfullyUnblocked
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}