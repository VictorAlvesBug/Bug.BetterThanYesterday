using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Payments.ConfirmPixPayment;

public sealed class ConfirmPixPaymentUseCase(
	IPenaltyRepository penaltyRepository,
	ILedgerRepository ledgerRepository,
	IPlanMemberRepository planMemberRepository)
	: IUseCase<ConfirmPixPaymentCommand>
{
	public async Task<IResult> HandleAsync(ConfirmPixPaymentCommand command)
	{
		command.Validate();

		var penalty = await penaltyRepository.GetByExternalPaymentIdAsync(command.ExternalPaymentId);

		if (penalty is null)
			return Result.Rejected(Messages.PenaltyNotFound, RejectionType.NotFound);

		if (penalty.Status != PenaltyStatus.Paid)
		{
			penalty.MarkAsPaid(PaymentMethod.Pix, null, command.ExternalPaymentId);
			await penaltyRepository.UpdateAsync(penalty);
		}

		await FinePaymentBook.RecordAsync(ledgerRepository, penalty);
		await UnblockIfClearAsync(penalty.PlanId, penalty.UserId);

		return Result.Success(PenaltyMapper.ToModel(penalty), Messages.PixPaymentConfirmed);
	}

	private async Task UnblockIfClearAsync(Guid planId, Guid userId)
	{
		if (await penaltyRepository.HasPendingPenaltiesAsync(planId, userId))
			return;

		var planMember = await planMemberRepository.GetByIdAsync(PlanMember.BuildId(planId, userId));

		if (planMember is null || planMember.Status != PlanMemberStatus.Blocked)
			return;

		planMember.MarkAsActive();
		await planMemberRepository.UpdateAsync(planMember);
	}
}
