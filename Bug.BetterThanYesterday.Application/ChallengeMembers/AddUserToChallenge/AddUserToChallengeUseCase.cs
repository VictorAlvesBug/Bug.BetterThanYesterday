using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.AddUserToChallenge;

public sealed class AddUserToChallengeUseCase(
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<AddUserToChallengeCommand>
{
    public async Task<IResult> HandleAsync(AddUserToChallengeCommand command)
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
                : await userRepository.GetByIdAsync(challenge.OwnerId);

            if (owner is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            if (challenge.GetStatus() != ChallengeStatus.NotStarted)
                return Result.Rejected(Messages.OnlyNotStartedChallengesCanReceiveNewMembers);

            var challengeMemberToAdd = ChallengeMember.CreateNew(command.ChallengeId, command.UserId);

            var challengeMemberDetailsModel = challengeMemberToAdd.ToChallengeMemberDetailsModel(habit, challenge, owner, user);

            var existingChallengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberToAdd.Id);

            if (existingChallengeMember is null)
            {
                await challengeMemberRepository.AddAsync(challengeMemberToAdd);
                return Result.Success(
                    challengeMemberDetailsModel,
                    Messages.MemberSuccessfullyAddedToTheChallenge
                );
            }

            return Result.Rejected(Messages.MemberAlreadyAddedToTheChallenge);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}