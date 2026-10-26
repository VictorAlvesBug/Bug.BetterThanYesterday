using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetChallengeUserDateWithCheckInsByChallengeIdAndUserIdAndDate;

public sealed class GetChallengeUserDateWithCheckInsByChallengeIdAndUserIdAndDateUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetChallengeUserDateWithCheckInsByChallengeIdAndUserIdAndDateCommand>
{
    public async Task<IResult> HandleAsync(GetChallengeUserDateWithCheckInsByChallengeIdAndUserIdAndDateCommand command)
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

            var checkIns = await checkInRepository.ListByChallengeIdAndUserIdAndDateAsync(
                command.ChallengeId,
                command.UserId,
                DateOnly.FromDateTime(command.Date)
            );

            if (!checkIns.Any())
                return Result.Success(
                    challenge.ToChallengeUserDateWithCheckInsModel(habit, owner, user, command.Date, []),
                    Messages.CheckInNotFound
                );

            return Result.Success(
                challenge.ToChallengeUserDateWithCheckInsModel(habit, owner, user, command.Date, checkIns.ToList()),
                Messages.CheckInsSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}