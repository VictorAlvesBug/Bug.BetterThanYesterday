using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.PayPenalty;

public sealed class PayPenaltyUseCase(
	IPenaltyRepository penaltyRepository,
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	ILedgerRepository ledgerRepository)
	: IUseCase<PayPenaltyCommand>
{
	public async Task<IResult> HandleAsync(PayPenaltyCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var penalty = await penaltyRepository.GetByIdAsync(command.PenaltyId);

			if (penalty is null || penalty.PlanId != command.PlanId)
				return Result.Rejected(Messages.PenaltyNotFound, RejectionType.NotFound);

			penalty.MarkAsPaid(PaymentMethod.Manual, command.ConfirmedByUserId);
			await penaltyRepository.UpdateAsync(penalty);
			await FinePaymentBook.RecordAsync(ledgerRepository, penalty);

			var planMemberId = PlanMember.BuildId(penalty.PlanId, penalty.UserId);
			var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

			if (planMember is not null
				&& planMember.Status == PlanMemberStatus.Blocked
				&& !await penaltyRepository.HasPendingPenaltiesAsync(penalty.PlanId, penalty.UserId))
			{
				planMember.MarkAsActive();
				await planMemberRepository.UpdateAsync(planMember);
			}

			return Result.Success(PenaltyMapper.ToModel(penalty), Messages.PenaltySuccessfullyPaid);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
