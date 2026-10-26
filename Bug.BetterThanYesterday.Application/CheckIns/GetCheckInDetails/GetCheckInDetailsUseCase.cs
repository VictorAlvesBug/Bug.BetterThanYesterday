using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public sealed class GetCheckInDetailsUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetCheckInDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInDetailsCommand command)
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

            var checkIn = await checkInRepository.GetDetailsAsync(
                command.ChallengeId,
                command.UserId,
                DateOnly.FromDateTime(command.Date),
                command.Index
            );

            if (checkIn is null)
                return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

            await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync([checkIn], checkInRepository);

            return Result.Success(
                checkIn.ToModel(challenge, habit, user),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}