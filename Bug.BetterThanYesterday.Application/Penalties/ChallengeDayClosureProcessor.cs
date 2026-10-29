using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Penalties;

public sealed class ChallengeDayClosureProcessor(
	ICheckInRepository checkInRepository,
	IPenaltyRepository penaltyRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IChallengeRepository challengeRepository)
	: IChallengeDayClosureProcessor
{
	// Cria as multas conforme necessidade
    public async Task ProcessChallengeAsync(Guid challengeId)
	{
		var challenge = await challengeRepository.GetByIdAsync(challengeId);

		if (challenge is null)
			return;

		var membersForQuorum = await challengeMemberRepository.ListByChallengeIdAsync(challengeId);
		await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(challenge, membersForQuorum.Count, challengeRepository);

		if (challenge.GetStatus() != ChallengeStatus.Running)
			return;

		var today = DateOnly.FromDateTime(DateTime.Today);
		var endDate = today.AddDays(-2);

		if (endDate < challenge.StartsAt)
			return;

		if (endDate > challenge.EndsAt)
			endDate = challenge.EndsAt;

		var challengeCheckIns = await checkInRepository.ListByChallengeIdAsync(challengeId);
		await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(challengeCheckIns, checkInRepository);

		var challengePenalties = await penaltyRepository.ListByChallengeIdAsync(challengeId);
		var members = (await challengeMemberRepository.ListByChallengeIdAsync(challengeId))
			.Where(m => m.Status == ChallengeMemberStatus.Active || m.Status == ChallengeMemberStatus.Blocked)
			.ToList();

		foreach (var member in members)
		{
			var memberCheckIns = challengeCheckIns.Where(c => c.UserId == member.UserId).ToList();
			var memberPenalties = challengePenalties.Where(p => p.UserId == member.UserId).ToList();
			var startDate = challenge.StartsAt < member.JoinedAt ? member.JoinedAt : challenge.StartsAt;

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
					challenge,
					memberCheckIns,
					memberPenalties);

				// Ainda possui folgas disponíveis					
                if (autoRestsBeforeDay < challenge.DaysOffPerWeek)
					continue;

				// Cria a multa para o membro nesse dia					
                var penalty = Penalty.CreateNew(challengeId: challengeId, userId: member.UserId, date: day, amount: challenge.PenaltyValue);
				await penaltyRepository.AddAsync(penalty);
				memberPenalties.Add(penalty);

				// Bloqueia o membro se estiver ativo
                if (member.Status == ChallengeMemberStatus.Active)
				{
					member.MarkAsBlocked();
					await challengeMemberRepository.UpdateAsync(member);
				}
			}
		}
	}
}
