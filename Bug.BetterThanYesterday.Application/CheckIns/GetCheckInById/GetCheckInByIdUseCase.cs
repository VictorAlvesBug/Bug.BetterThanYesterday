using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInById;

public sealed class GetCheckInByIdUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IChallengeMemberRepository challengeMemberRepository)
    : IUseCase<GetCheckInByIdCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInByIdCommand command)
	{
		try
		{
			command.Validate();
			
			var checkIn = await checkInRepository.GetByIdAsync(command.CheckInId);

			if (checkIn is null)
				return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

			await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync([checkIn], checkInRepository);

			var challenge = await challengeRepository.GetByIdAsync(checkIn.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(challenge.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var user = await userRepository.GetByIdAsync(checkIn.UserId);

			if (user is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			return Result.Success(
                await checkIn.ToModelAsync(
					challengeRepository,
					habitRepository,
					userRepository
				)
            );
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}