using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Payments.ConfirmPixPayment;

public sealed class ConfirmPixPaymentUseCase(
	IPenaltyRepository penaltyRepository,
	ILedgerRepository ledgerRepository,
	IChallengeMemberRepository challengeMemberRepository)
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
		await UnblockIfClearAsync(penalty.ChallengeId, penalty.UserId);

		return Result.Success(PenaltyMapper.ToModel(penalty), Messages.PixPaymentConfirmed);
	}

	private async Task UnblockIfClearAsync(Guid challengeId, Guid userId)
	{
		if (await penaltyRepository.HasPendingPenaltiesAsync(challengeId, userId))
			return;

		var challengeMember = await challengeMemberRepository.GetByIdAsync(ChallengeMember.BuildId(challengeId, userId));

		if (challengeMember is null || challengeMember.Status != ChallengeMemberStatus.Blocked)
			return;

		challengeMember.MarkAsActive();
		await challengeMemberRepository.UpdateAsync(challengeMember);
	}
}
