using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Ledger;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.PayPenalty;

public sealed class PayPenaltyUseCase(
	IPenaltyRepository penaltyRepository,
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	ILedgerRepository ledgerRepository)
	: IUseCase<PayPenaltyCommand>
{
	public async Task<IResult> HandleAsync(PayPenaltyCommand command)
	{
		try
		{
			command.Validate();

			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var penalty = await penaltyRepository.GetByIdAsync(command.PenaltyId);

			if (penalty is null || penalty.ChallengeId != command.ChallengeId)
				return Result.Rejected(Messages.PenaltyNotFound, RejectionType.NotFound);

			penalty.MarkAsPaid(PaymentMethod.Manual, command.ConfirmedByUserId);
			await penaltyRepository.UpdateAsync(penalty);
			await FinePaymentBook.RecordAsync(ledgerRepository, penalty);

			var challengeMemberId = ChallengeMember.BuildId(penalty.ChallengeId, penalty.UserId);
			var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

			if (challengeMember is not null
				&& challengeMember.Status == ChallengeMemberStatus.Blocked
				&& !await penaltyRepository.HasPendingPenaltiesAsync(penalty.ChallengeId, penalty.UserId))
			{
				challengeMember.MarkAsActive();
				await challengeMemberRepository.UpdateAsync(challengeMember);
			}

			return Result.Success(PenaltyMapper.ToModel(penalty), Messages.PenaltySuccessfullyPaid);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
