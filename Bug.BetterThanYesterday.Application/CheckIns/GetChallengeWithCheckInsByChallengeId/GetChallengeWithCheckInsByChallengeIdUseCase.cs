using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetChallengeWithCheckInsByChallengeId;

public sealed class GetChallengeWithCheckInsByChallengeIdUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetChallengeWithCheckInsByChallengeIdCommand>
{
    public async Task<IResult> HandleAsync(GetChallengeWithCheckInsByChallengeIdCommand command)
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

            var checkIns = await checkInRepository.ListByChallengeIdAsync(command.ChallengeId);
            var userIds = checkIns.Select(checkIn => checkIn.UserId).Distinct().ToList();
            var users = userIds.Count == 0
                ? []
                : await userRepository.BatchGetByIdAsync(userIds);

            return Result.Success(
                challenge.ToChallengeWithCheckInsModel(habit, owner, checkIns, users),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}