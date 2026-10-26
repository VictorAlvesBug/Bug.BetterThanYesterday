using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Payments;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Payments.CreatePixCharge;

public sealed class CreatePixChargeUseCase(
	IPenaltyRepository penaltyRepository,
	IPixGateway pixGateway)
	: IUseCase<CreatePixChargeCommand>
{
	public async Task<IResult> HandleAsync(CreatePixChargeCommand command)
	{
		command.Validate();

		var penalty = await penaltyRepository.GetByIdAsync(command.PenaltyId);

		if (penalty is null || penalty.ChallengeId != command.ChallengeId)
			return Result.Rejected(Messages.PenaltyNotFound, RejectionType.NotFound);

		if (penalty.UserId != command.UserId)
			return Result.Rejected(Messages.OnlyPenaltyOwnerCanCreatePixCharge);

		if (penalty.Status != PenaltyStatus.Pending)
			return Result.Rejected(Messages.PenaltyAlreadyPaid);

		var charge = await pixGateway.CreateChargeAsync(penalty.Amount, "Multa do desafio", penalty.Id);
		penalty.AssignCharge(charge.ExternalId);
		await penaltyRepository.UpdateAsync(penalty);

		return Result.Success(
			new { charge.ExternalId, charge.CopyAndPaste, penalty.Amount },
			Messages.PixChargeSuccessfullyCreated);
	}
}
