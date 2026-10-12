using Bug.BetterThanYesterday.Application.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Ledger.Entities;
using Bug.BetterThanYesterday.Domain.Ledger.ValueObjects;
using Bug.BetterThanYesterday.Domain.Payments;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Ledger;

public interface IPlanFinancesProcessor
{
	Task ProcessAsync(Guid planId);
}

public sealed class PlanFinancesProcessor(
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	IPenaltyRepository penaltyRepository,
	ICheckInRepository checkInRepository,
	ILedgerRepository ledgerRepository,
	IUserRepository userRepository,
	IPixGateway pixGateway)
	: IPlanFinancesProcessor
{
	public async Task ProcessAsync(Guid planId)
	{
		var plan = await planRepository.GetByIdAsync(planId);

		if (plan is null)
			return;

		var status = plan.GetStatus();

		if (status == PlanStatus.Cancelled)
		{
			await RefundPaidFinesAsync(plan);
			return;
		}

		if (status != PlanStatus.Finished)
			return;

		await ForgiveUnpaidFinesAsync(plan);
		await SettleAsync(plan);
	}

	private async Task RefundPaidFinesAsync(Plan plan)
	{
		var penalties = await penaltyRepository.ListByPlanIdAsync(plan.Id);
		var ledger = await ledgerRepository.ListByPlanIdAsync(plan.Id);
		var refundedPenaltyIds = ledger
			.Where(entry => entry.Kind == LedgerKind.Refund && entry.PenaltyId is not null)
			.Select(entry => entry.PenaltyId!.Value)
			.ToHashSet();
		var created = new List<LedgerEntry>();

		foreach (var penalty in penalties.Where(penalty => penalty.Status == PenaltyStatus.Paid))
		{
			if (refundedPenaltyIds.Contains(penalty.Id))
				continue;

			var entry = LedgerEntry.CreateRefund(plan.Id, penalty.UserId, penalty.Id, penalty.Date, penalty.Amount);
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

	private async Task ForgiveUnpaidFinesAsync(Plan plan)
	{
		var penalties = await penaltyRepository.ListByPlanIdAsync(plan.Id);
		var members = await planMemberRepository.ListByPlanIdAsync(plan.Id);

		foreach (var penalty in penalties.Where(penalty => penalty.Status == PenaltyStatus.Pending))
		{
			penalty.Forgive();
			await penaltyRepository.UpdateAsync(penalty);

			var member = members.FirstOrDefault(candidate => candidate.UserId == penalty.UserId);

			if (member is null || member.IsDelinquent)
				continue;

			member.MarkAsDelinquent();
			await planMemberRepository.UpdateAsync(member);
		}
	}

	private async Task SettleAsync(Plan plan)
	{
		var ledger = await ledgerRepository.ListByPlanIdAsync(plan.Id);
		var owedSettlements = ledger
			.Where(entry => entry.Kind == LedgerKind.Settlement && entry.Status == LedgerStatus.Owed)
			.ToList();

		if (ledger.Any(entry => entry.Kind == LedgerKind.Settlement))
		{
			await PayOutAsync(owedSettlements);
			return;
		}

		var checkIns = await checkInRepository.ListByPlanIdAsync(plan.Id);
		await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(checkIns, checkInRepository);

		if (checkIns.Any(checkIn => checkIn.IsReviewWindowOpen()))
			return;

		var members = await planMemberRepository.ListByPlanIdAsync(plan.Id);
		var penalties = await penaltyRepository.ListByPlanIdAsync(plan.Id);
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
			var entry = LedgerEntry.CreateSettlement(plan.Id, userId, amount);
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
