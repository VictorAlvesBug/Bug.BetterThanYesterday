using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetChallengeUserWithCheckIns;

public sealed class GetChallengeUserWithCheckInsUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetChallengeUserWithCheckInsCommand>
{
    public async Task<IResult> HandleAsync(GetChallengeUserWithCheckInsCommand command)
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

            var checkIns = await checkInRepository.ListByChallengeIdAndUserIdAsync(
                command.ChallengeId,
                command.UserId
            );

            if (!checkIns.Any())
                return Result.Success(
                    challenge.ToChallengeUserWithCheckInsModel(habit, owner, user, []),
                    Messages.CheckInNotFound
                );

            return Result.Success(
                challenge.ToChallengeUserWithCheckInsModel(habit, owner, user, checkIns.ToList()),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}