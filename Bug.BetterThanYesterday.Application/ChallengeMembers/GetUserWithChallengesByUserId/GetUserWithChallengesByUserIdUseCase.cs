using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.ChallengeMembers.GetUserWithChallengesByUserId;

public sealed class GetUserWithChallengesByUserIdUseCase(
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPenaltyRepository penaltyRepository)
    : IUseCase<GetUserWithChallengesByUserIdCommand>
{
    public async Task<IResult> HandleAsync(GetUserWithChallengesByUserIdCommand command)
    {
        try
        {
            command.Validate();
            
            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var challengeMembers = await challengeMemberRepository.ListByUserIdAsync(command.UserId);

            if (challengeMembers.Count == 0)
                return Result.Success(user.ToUserWithChallengesModel(), Messages.UserHasNoChallenges);

            var challengeIds = challengeMembers.Select(challengeMember => challengeMember.ChallengeId).ToList();

            var challenges = await challengeRepository.BatchGetByIdAsync(challengeIds);

            if (challengeIds.Count > challenges.Count)
            {
                var notFoundIds = challengeIds.Where(id => !challenges.Any(p => p.Id == id));
                var strNotFoundIds = string.Join(", ", notFoundIds);
				return Result.Rejected($"Esses desafios sumiram do mapa: {strNotFoundIds}");
            }

            var ownerIds = challenges.Select(challenge => challenge.OwnerId).Distinct().ToList();
            var owners = ownerIds.Count == 0
                ? []
                : await userRepository.BatchGetByIdAsync(ownerIds);

            if (ownerIds.Count > owners.Count)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var ownersById = owners.ToDictionary(owner => owner.Id);

            var tasks = challenges.Select(async challenge => {
                var habit = await habitRepository.GetByIdAsync(challenge.HabitId) ?? throw new Exception(Messages.HabitNotFound);
                var model = challenge.ToModel(habit, ownersById[challenge.OwnerId]);
                var penalties = await penaltyRepository.ListByChallengeIdAsync(challenge.Id);
                model.AmountOwed = penalties
                    .Where(penalty => penalty.UserId == command.UserId && penalty.Status == PenaltyStatus.Pending)
                    .Sum(penalty => penalty.Amount);
                var paidPot = penalties
                    .Where(penalty => penalty.Status == PenaltyStatus.Paid)
                    .Sum(penalty => penalty.Amount);
                model.RewardPool = SettlementCalculator.DistributableAmount(paidPot);
                return model;
            }).ToList();

            var challengeModels = (await Task.WhenAll(tasks)).ToList();

            return Result.Success(
                user.ToUserWithChallengesModel(challengeModels),
                Messages.UserSuccessfullyFound);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}