using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public sealed class AddCheckInUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeCycleNotificationProcessor notificationProcessor)
    : IUseCase<AddCheckInCommand>
{
    public async Task<IResult> HandleAsync(AddCheckInCommand command)
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

            var challengeMemberId = ChallengeMember.BuildId(
                command.ChallengeId,
                command.UserId
            );
            var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

            if (challengeMember is null)
                return Result.Rejected(Messages.ChallengeMemberNotFound, RejectionType.NotFound);

            var checkIns = await checkInRepository.ListByChallengeIdAndUserIdAndDateAsync(
                command.ChallengeId,
                command.UserId,
                DateOnly.FromDateTime(command.Date)
            );

            var nextIndex = checkIns.Any()
                ? checkIns.Max(ci => ci.Index) + 1
                : 1;

            var maxIndexPerDateAllowed = challenge.GetMaxCheckInsPerDateAllowed();

            if (challengeMember.Status != ChallengeMemberStatus.Active)
                return Result.Rejected(Messages.OnlyActiveMembersCanMakeCheckIns);

            if (challenge.GetStatus() != ChallengeStatus.Running)
                return Result.Rejected(Messages.OnlyRunningChallengesCanReceiveNewCheckIns);

            var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);

            if (members.Count < 2)
                return Result.Rejected(Messages.ChallengeNeedsAtLeastTwoMembers);

            var checkInDay = DateOnly.FromDateTime(command.Date);
            var today = DateOnly.FromDateTime(DateTime.Today);
            var yesterday = today.AddDays(-1);

            if (checkInDay != today && checkInDay != yesterday)
                return Result.Rejected(Messages.CheckInDateMustBeTodayOrYesterday);

            if (command.Date < challenge.StartsAt.ToDateTime(TimeOnly.MinValue) || command.Date > challenge.EndsAt.ToDateTime(TimeOnly.MaxValue))
                return Result.Rejected(Messages.OnlyRunningChallengesCanReceiveNewCheckIns);

            if (nextIndex > maxIndexPerDateAllowed)
                return Result.Rejected(Messages.UserHasReachedTheMaximumNumberOfCheckInsForTheDay);
                
            var checkIn = CheckIn.CreateNew(
                command.ChallengeId,
                command.UserId,
                command.Date,
                nextIndex,
                command.Title,
                command.EvidencePhotoUrl);

            await checkInRepository.AddAsync(checkIn);

            try
            {
				await notificationProcessor.NotifyCheckInAsync(challenge.Id, checkIn.Id, user.Id, user.Name, members);
            }
            catch
            {
                // O aviso não impede o check-in.
            }

            return Result.Success(
                checkIn.ToModel(challenge, habit, user),
                Messages.CheckInSuccessfullyRegistered
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}