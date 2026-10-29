using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Ledger.Entities;
using Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;
using Bug.BetterThanYesterday.Domain.Payments;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Ledger;

public sealed class ChallengeFinancesProcessor(
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	IPenaltyRepository penaltyRepository,
	ICheckInRepository checkInRepository,
	ILedgerRepository ledgerRepository,
	IUserRepository userRepository,
	IPixGateway pixGateway)
	: IChallengeFinancesProcessor
{
	public async Task ProcessAsync(Guid challengeId)
	{
		var challenge = await challengeRepository.GetByIdAsync(challengeId);

		if (challenge is null)
			return;

		var status = challenge.GetStatus();

		if (status == ChallengeStatus.Cancelled)
		{
			await RefundPaidFinesAsync(challenge);
			return;
		}

		if (status != ChallengeStatus.Finished)
			return;

		await ForgiveUnpaidFinesAsync(challenge);
		await SettleAsync(challenge);
	}

	private async Task RefundPaidFinesAsync(Challenge challenge)
	{
		var penalties = await penaltyRepository.ListByChallengeIdAsync(challenge.Id);
		var ledger = await ledgerRepository.ListByChallengeIdAsync(challenge.Id);
		var refundedPenaltyIds = ledger
			.Where(entry => entry.Kind == LedgerKind.Refund && entry.PenaltyId is not null)
			.Select(entry => entry.PenaltyId!.Value)
			.ToHashSet();
		var created = new List<LedgerEntry>();

		foreach (var penalty in penalties.Where(penalty => penalty.Status == PenaltyStatus.Paid))
		{
			if (refundedPenaltyIds.Contains(penalty.Id))
				continue;

			var entry = LedgerEntry.CreateRefund(challenge.Id, penalty.UserId, penalty.Id, penalty.Date, penalty.Amount);
			await ledgerRepository.AddAsync(entry);
			created.Add(entry);
		}

		foreach (var penalty in penalties.Where(penalty => penalty.Status == PenaltyStatus.Pending))
		{
			penalty.Forgive();
			await penaltyRepository.UpdateAsync(penalty);
		}

		await PayOutAsync(created);
	}

	private async Task ForgiveUnpaidFinesAsync(Challenge challenge)
	{
		var penalties = await penaltyRepository.ListByChallengeIdAsync(challenge.Id);
		var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);

		foreach (var penalty in penalties.Where(penalty => penalty.Status == PenaltyStatus.Pending))
		{
			penalty.Forgive();
			await penaltyRepository.UpdateAsync(penalty);

			var member = members.FirstOrDefault(candidate => candidate.UserId == penalty.UserId);

			if (member is null || member.IsDelinquent)
				continue;

			member.MarkAsDelinquent();
			await challengeMemberRepository.UpdateAsync(member);
		}
	}

	private async Task SettleAsync(Challenge challenge)
	{
		var ledger = await ledgerRepository.ListByChallengeIdAsync(challenge.Id);
		var owedSettlements = ledger
			.Where(entry => entry.Kind == LedgerKind.Settlement && entry.Status == LedgerStatus.Owed)
			.ToList();

		if (ledger.Any(entry => entry.Kind == LedgerKind.Settlement))
		{
			await PayOutAsync(owedSettlements);
			return;
		}

		var checkIns = await checkInRepository.ListByChallengeIdAsync(challenge.Id);
		await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(checkIns, checkInRepository);

		if (checkIns.Any(checkIn => checkIn.IsReviewWindowOpen()))
			return;

		var members = await challengeMemberRepository.ListByChallengeIdAsync(challenge.Id);
		var penalties = await penaltyRepository.ListByChallengeIdAsync(challenge.Id);
		var pot = penalties.Where(penalty => penalty.Status == PenaltyStatus.Paid).Sum(penalty => penalty.Amount);
		var counts = members
			.Select(member => (
				member.UserId,
				ValidatedCheckIns: checkIns.Count(checkIn =>
					checkIn.UserId == member.UserId && checkIn.ResolveStatus() == CheckInStatus.Validated)))
			.ToList();
		var created = new List<LedgerEntry>();

		foreach (var (userId, amount) in SettlementCalculator.Split(pot, counts))
		{
			var entry = LedgerEntry.CreateSettlement(challenge.Id, userId, amount);
			await ledgerRepository.AddAsync(entry);
			created.Add(entry);
		}

		await PayOutAsync(created);
	}

	private async Task PayOutAsync(IReadOnlyList<LedgerEntry> entries)
	{
		if (entries.Count == 0)
			return;

		var users = await userRepository.BatchGetByIdAsync(entries.Select(entry => entry.UserId).Distinct().ToList());

		foreach (var entry in entries)
		{
			var user = users.FirstOrDefault(candidate => candidate.Id == entry.UserId);

			if (user is null || string.IsNullOrWhiteSpace(user.PixKey.Value))
				continue;

			try
			{
				var transfer = await pixGateway.SendAsync(user.PixKey.Value, entry.Amount, entry.Kind.Name);
				entry.MarkPaid(transfer.ExternalId);
				await ledgerRepository.UpdateAsync(entry);
			}
			catch
			{
				// A saída fica devida até a próxima tentativa.
			}
		}
	}
}
