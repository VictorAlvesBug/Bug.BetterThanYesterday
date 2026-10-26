using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.GetChallengeWithMembersByChallengeId;

public sealed class GetChallengeWithMembersByChallengeIdUseCase(
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetChallengeWithMembersByChallengeIdCommand>
{
    public async Task<IResult> HandleAsync(GetChallengeWithMembersByChallengeIdCommand command)
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

            var owner = await userRepository.GetByIdAsync(challenge.OwnerId);

            if (owner is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var challengeMembers = await challengeMemberRepository.ListByChallengeIdAsync(command.ChallengeId);

            if (challengeMembers.Count == 0)
                return Result.Success(challenge.ToChallengeWithMembersModel(habit, owner), Messages.ChallengeHasNoMembers);

            var memberIds = challengeMembers.Select(challengeMember => challengeMember.UserId).ToList();

            var users = await userRepository.BatchGetByIdAsync(memberIds);

            if (memberIds.Count > users.Count)
            {
                var notFoundIds = memberIds.Where(id => !users.Any(p => p.Id == id));
                var strNotFoundIds = string.Join(", ", notFoundIds);
                return Result.Rejected($"Esses jogadores sumiram do mapa: {strNotFoundIds}");
            }

            return Result.Success(
                challenge.ToChallengeWithMembersModel(habit, owner, users),
                Messages.ChallengeSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}