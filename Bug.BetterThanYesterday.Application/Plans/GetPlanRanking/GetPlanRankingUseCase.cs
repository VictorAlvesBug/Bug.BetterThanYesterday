using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.Plans.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanRanking;

public sealed class GetPlanRankingUseCase(
    IPlanRepository planRepository,
    IPlanMemberRepository planMemberRepository,
    ICheckInRepository checkInRepository,
    IPenaltyRepository penaltyRepository,
    IUserRepository userRepository,
    IPlanDayClosureProcessor planDayClosureProcessor,
    IPlanFinancesProcessor planFinancesProcessor)
    : IUseCase<GetPlanRankingCommand>
{
    public async Task<IResult> HandleAsync(GetPlanRankingCommand command)
    {
        try
        {
            command.Validate();

            await planDayClosureProcessor.ProcessPlanAsync(command.PlanId);
            await planFinancesProcessor.ProcessAsync(command.PlanId);

            var plan = await planRepository.GetByIdAsync(command.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

            var today = DateOnly.FromDateTime(DateTime.Today);
            var elapsedEnd = plan.EndsAt < today ? plan.EndsAt : today;
            var totalCheckinCount = elapsedEnd >= plan.StartsAt
                ? elapsedEnd.DayNumber - plan.StartsAt.DayNumber + 1
                : 0;

            var members = (await planMemberRepository.ListByPlanIdAsync(command.PlanId))
                .Where(m => m.Status == PlanMemberStatus.Active || m.Status == PlanMemberStatus.Blocked)
                .ToList();

            var planCheckIns = await checkInRepository.ListByPlanIdAsync(command.PlanId);
            await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(planCheckIns, checkInRepository);

            var planPenalties = await penaltyRepository.ListByPlanIdAsync(command.PlanId);

            var items = new List<PlanRankingItemModel>();

            foreach (var member in members)
            {
                var user = await userRepository.GetByIdAsync(member.UserId);

                if (user is null)
                    continue;

                var memberCheckIns = planCheckIns.Where(c => c.UserId == member.UserId).ToList();
                var memberPenalties = planPenalties.Where(p => p.UserId == member.UserId).ToList();

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
                    plan,
                    memberCheckIns,
                    memberPenalties,
                    today);

                items.Add(new PlanRankingItemModel
                {
                    UserId = member.UserId,
                    UserName = user.Nickname,
                    PhotoUrl = user.Photo?.Value,
                    CheckinCount = validatedCount,
                    PendingCheckinCount = pendingCheckinCount,
                    Penalty = pendingPenaltyAmount,
                    Streak = streak
                });
            }

            items = items
                .OrderByDescending(i => i.CheckinCount)
                .ThenBy(i => i.Penalty)
                .ThenByDescending(i => i.Streak)
                .ToList();

            for (var i = 0; i < items.Count; i++)
                items[i].Position = i + 1;

            PlanRankingItemModel? currentUser = null;
            var daysOffAvailable = 0;

            if (command.UserId is not null)
            {
                currentUser = items.FirstOrDefault(i => i.UserId == command.UserId.Value);
                var member = members.FirstOrDefault(m => m.UserId == command.UserId.Value);

                if (member is not null)
                {
                    var userCheckIns = planCheckIns.Where(c => c.UserId == command.UserId.Value).ToList();
                    var userPenalties = planPenalties.Where(p => p.UserId == command.UserId.Value).ToList();
                    daysOffAvailable = DayOffAvailabilityCalculator.GetDaysOffAvailable(
                        plan,
                        member.JoinedAt,
                        today,
                        userCheckIns,
                        userPenalties);
                }
            }

            return Result.Success(
                new PlanRankingModel
                {
                    TotalCheckinCount = totalCheckinCount,
                    DaysOffAvailable = daysOffAvailable,
                    Items = items,
                    CurrentUser = currentUser
                },
                Messages.PlanRankingSuccessfullyFound);
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
        Domain.Plans.Entities.Plan plan,
        IReadOnlyList<CheckIn> memberCheckIns,
        IReadOnlyList<Penalty> memberPenalties,
        DateOnly today)
    {
        var pendingCheckinsRemaining = pendingCheckinCount;
        var streak = 0;
        var cursor = today;

        // Itera dias para trás: de hoje até o início
        while (cursor >= plan.StartsAt && cursor >= memberJoinedAt)
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
                plan,
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
}
