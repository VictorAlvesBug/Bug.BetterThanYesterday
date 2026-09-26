using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Penalties;

public interface IPlanDayClosureProcessor
{
	Task ProcessPlanAsync(Guid planId);
}

public sealed class PlanDayClosureProcessor(
	ICheckInRepository checkInRepository,
	IPenaltyRepository penaltyRepository,
	IPlanMemberRepository planMemberRepository,
	IPlanRepository planRepository)
	: IPlanDayClosureProcessor
{
	// Cria as multas conforme necessidade
    public async Task ProcessPlanAsync(Guid planId)
	{
		var plan = await planRepository.GetByIdAsync(planId);

		if (plan is null || plan.GetStatus() != PlanStatus.Running)
			return;

		var today = DateOnly.FromDateTime(DateTime.Today);
		var endDate = today.AddDays(-1);

		if (endDate < plan.StartsAt)
			return;

		if (endDate > plan.EndsAt)
			endDate = plan.EndsAt;

		var planCheckIns = await checkInRepository.ListByPlanIdAsync(planId);
		await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(planCheckIns, checkInRepository);

		var planPenalties = await penaltyRepository.ListByPlanIdAsync(planId);
		var members = (await planMemberRepository.ListByPlanIdAsync(planId))
			.Where(m => m.Status == PlanMemberStatus.Active || m.Status == PlanMemberStatus.Blocked)
			.ToList();

		foreach (var member in members)
		{
			var memberCheckIns = planCheckIns.Where(c => c.UserId == member.UserId).ToList();
			var memberPenalties = planPenalties.Where(p => p.UserId == member.UserId).ToList();
			var startDate = plan.StartsAt < member.JoinedAt ? member.JoinedAt : plan.StartsAt;

			for (var day = startDate; day <= endDate; day = day.AddDays(1))
            {
            	// Existem multas para o membro nesse dia
                if (memberPenalties.Any(p => p.Date == day))
					continue;

				var coverage = DayOffAvailabilityCalculator.EvaluateDayCoverage(day, memberCheckIns);

				// Checkin ainda em período de avaliação ou pendente	
                if (coverage == DayCoverage.Deferred)
					continue;

				// Checkin já validado					
                if (coverage == DayCoverage.Covered)
					continue;

				// Contagem dos dias de folga utilizados nos últimos 6 dias antes do dia informado
                var autoRestsBeforeDay = DayOffAvailabilityCalculator.CountAutoRestDaysBeforeDay(
					day,
					member.JoinedAt,
					plan,
					memberCheckIns,
					memberPenalties);

				// Ainda possui folgas disponíveis					
                if (autoRestsBeforeDay < plan.DaysOffPerWeek)
					continue;

				// Cria a multa para o membro nesse dia					
                var penalty = Penalty.CreateNew(planId, member.UserId, day, plan.PenaltyValue);
				await penaltyRepository.AddAsync(penalty);
				memberPenalties.Add(penalty);

				// Bloqueia o membro se estiver ativo
                if (member.Status == PlanMemberStatus.Active)
				{
					member.MarkAsBlocked();
					await planMemberRepository.UpdateAsync(member);
				}
			}
		}
	}
}
