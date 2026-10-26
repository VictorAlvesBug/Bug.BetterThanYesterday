using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.Challenges.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Challenges.GetChallengeRanking;

public sealed class GetChallengeRankingUseCase(
    IChallengeRepository challengeRepository,
    IChallengeMemberRepository challengeMemberRepository,
    ICheckInRepository checkInRepository,
    IPenaltyRepository penaltyRepository,
    IUserRepository userRepository,
    IChallengeDayClosureProcessor challengeDayClosureProcessor,
    IChallengeFinancesProcessor challengeFinancesProcessor)
    : IUseCase<GetChallengeRankingCommand>
{
    public async Task<IResult> HandleAsync(GetChallengeRankingCommand command)
    {
        try
        {
            command.Validate();

            await challengeDayClosureProcessor.ProcessChallengeAsync(command.ChallengeId);
            await challengeFinancesProcessor.ProcessAsync(command.ChallengeId);

            var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

            if (challenge is null)
                return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

            var today = DateOnly.FromDateTime(DateTime.Today);
            var elapsedEnd = challenge.EndsAt < today ? challenge.EndsAt : today;
            var totalCheckinCount = elapsedEnd >= challenge.StartsAt
                ? elapsedEnd.DayNumber - challenge.StartsAt.DayNumber + 1
                : 0;

            var members = (await challengeMemberRepository.ListByChallengeIdAsync(command.ChallengeId))
                .Where(m => m.Status == ChallengeMemberStatus.Active || m.Status == ChallengeMemberStatus.Blocked)
                .ToList();

            var challengeCheckIns = await checkInRepository.ListByChallengeIdAsync(command.ChallengeId);
            await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(challengeCheckIns, checkInRepository);

            var challengePenalties = await penaltyRepository.ListByChallengeIdAsync(command.ChallengeId);

            var items = new List<ChallengeRankingItemModel>();

            foreach (var member in members)
            {
                var user = await userRepository.GetByIdAsync(member.UserId);

                if (user is null)
                    continue;

                var memberCheckIns = challengeCheckIns.Where(c => c.UserId == member.UserId).ToList();
                var memberPenalties = challengePenalties.Where(p => p.UserId == member.UserId).ToList();

                var validatedDates = memberCheckIns
                    .Where(c => c.ResolveStatus() == CheckInStatus.Validated)
                    .Select(c => c.CalendarDate)
                    .ToHashSet();

                var validatedCount = validatedDates.Count;
                var pendingCheckinCount = memberCheckIns
                    .Count(c => c.ResolveStatus() == CheckInStatus.Pending
                        && c.CalendarDate <= elapsedEnd);

                // Valor a pagar de multas pendentes
                var pendingPenaltyAmount = memberPenalties
                    .Where(p => p.Status == PenaltyStatus.Pending)
                    .Sum(p => p.Amount);

                var streak = CalculateStreak(
                    validatedDates,
                    pendingCheckinCount,
                    member.JoinedAt,
                    challenge,
                    memberCheckIns,
                    memberPenalties,
                    today);

                items.Add(new ChallengeRankingItemModel
                {
                    UserId = member.UserId,
                    UserName = user.Nickname,
                    PhotoUrl = user.Photo?.Value,
                    CheckinCount = validatedCount,
                    PendingCheckinCount = pendingCheckinCount,
                    Penalty = pendingPenaltyAmount,
                    Streak = streak,
                    Days = BuildRankingDays(challenge, today, memberCheckIns)
                });
            }

            items = items
                .OrderByDescending(i => i.CheckinCount)
                .ThenBy(i => i.Penalty)
                .ThenByDescending(i => i.Streak)
                .ToList();

            for (var i = 0; i < items.Count; i++)
                items[i].Position = i + 1;

            ChallengeRankingItemModel? currentUser = null;
            var daysOffAvailable = 0;

            if (command.UserId is not null)
            {
                currentUser = items.FirstOrDefault(i => i.UserId == command.UserId.Value);
                var member = members.FirstOrDefault(m => m.UserId == command.UserId.Value);

                if (member is not null)
                {
                    var userCheckIns = challengeCheckIns.Where(c => c.UserId == command.UserId.Value).ToList();
                    var userPenalties = challengePenalties.Where(p => p.UserId == command.UserId.Value).ToList();
                    daysOffAvailable = DayOffAvailabilityCalculator.GetDaysOffAvailable(
                        challenge,
                        member.JoinedAt,
                        today,
                        userCheckIns,
                        userPenalties);
                }
            }

            return Result.Success(
                new ChallengeRankingModel
                {
                    TotalCheckinCount = totalCheckinCount,
                    DaysOffAvailable = daysOffAvailable,
                    Items = items,
                    CurrentUser = currentUser
                },
                Messages.ChallengeRankingSuccessfullyFound);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }

    public static int CalculateStreak(
        IReadOnlySet<DateOnly> validatedDates,
        int pendingCheckinCount,
        DateOnly memberJoinedAt,
        Domain.Challenges.Entities.Challenge challenge,
        IReadOnlyList<CheckIn> memberCheckIns,
        IReadOnlyList<Penalty> memberPenalties,
        DateOnly today)
    {
        var pendingCheckinsRemaining = pendingCheckinCount;
        var streak = 0;
        var cursor = today;

        // Itera dias para trás: de hoje até o início
        while (cursor >= challenge.StartsAt && cursor >= memberJoinedAt)
        {
            // Checkin do dia já está validado
            if (validatedDates.Contains(cursor))
            {
                streak++;
                cursor = cursor.AddDays(-1);
                continue;
            }

            // Checkin do dia está pendente
            if (pendingCheckinsRemaining > 0)
            {
                streak++;
                pendingCheckinsRemaining = Math.Max(0, pendingCheckinsRemaining - 1);
                cursor = cursor.AddDays(-1);
                continue;
            }

            // Dia é hoje e ainda não tem checkin realizados
            if (cursor == today && !validatedDates.Contains(cursor))
            {
                cursor = cursor.AddDays(-1);
                continue;
            }

            // Dia é elegível para ser considerado como folga
            if (DayOffAvailabilityCalculator.IsAutoRestDay(
                cursor,
                memberJoinedAt,
                challenge,
                memberCheckIns,
                memberPenalties))
            {
                streak++;
                cursor = cursor.AddDays(-1);
                continue;
            }

            break;
        }

        return streak;
    }

    private static List<RankingDayModel> BuildRankingDays(Domain.Challenges.Entities.Challenge challenge, DateOnly today, List<CheckIn> checkIns)
    {
        var duration = challenge.EndsAt.DayNumber - challenge.StartsAt.DayNumber + 1;
        if (duration < 1)
            return [];

        var windowSize = Math.Min(duration, 7);
        DateOnly windowStart;

        if (today <= challenge.StartsAt)
            windowStart = challenge.StartsAt;
        else if (today >= challenge.EndsAt)
            windowStart = challenge.EndsAt.AddDays(1 - windowSize);
        else
        {
            var daysSinceStart = today.DayNumber - challenge.StartsAt.DayNumber;
            windowStart = daysSinceStart < windowSize - 1
                ? challenge.StartsAt
                : today.AddDays(1 - windowSize);
        }

        if (windowStart < challenge.StartsAt)
            windowStart = challenge.StartsAt;

        var days = new List<RankingDayModel>();

        for (var index = 0; index < windowSize; index++)
        {
            var date = windowStart.AddDays(index);
            if (date > challenge.EndsAt)
                break;

            var dayCheckIns = checkIns.Where(checkIn => checkIn.CalendarDate == date).ToList();
            days.Add(new RankingDayModel
            {
                Date = date.ToDateTime(TimeOnly.MinValue),
                Position = date < today ? "Past" : date == today ? "Today" : "Future",
                Tone = ToneFor(date, today, dayCheckIns)
            });
        }

        return days;
    }

    private static string ToneFor(DateOnly date, DateOnly today, List<CheckIn> dayCheckIns)
    {
        if (date > today)
            return "gray-e";

        if (dayCheckIns.Count == 0)
            return date < today.AddDays(-1) ? "danger" : "gray-e";

        var chosen = dayCheckIns.FirstOrDefault(checkIn => checkIn.ResolveStatus() == CheckInStatus.Validated)
            ?? dayCheckIns.FirstOrDefault(checkIn => checkIn.IsReviewWindowOpen())
            ?? dayCheckIns[0];

        if (chosen.IsReviewWindowOpen())
        {
            var activeReviews = chosen.Reviews.Where(review => review.Enable).ToList();
            var rejections = activeReviews.Count(review => review.Status == CheckInStatus.Rejected);
            return activeReviews.Count > 0 && rejections > activeReviews.Count / 2
                ? "dark-orange"
                : "gold";
        }

        return chosen.ResolveStatus() == CheckInStatus.Rejected ? "danger" : "success";
    }
}
