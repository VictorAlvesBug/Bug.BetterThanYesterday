using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Utils;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

public class PlanParticipant : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public DateOnly? LeftAt { get; set; }
	public PlanParticipantStatus Status { get; set; }

	private PlanParticipant(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		DateTime? leftAt,
		int statusId)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		JoinedAt = DateOnly.FromDateTime(joinedAt);
		LeftAt = leftAt is null ? null : DateOnly.FromDateTime(leftAt.Value);
		Status = PlanParticipantStatus.FromId(statusId);
	}

	private PlanParticipant(Guid planId, Guid userId)
		: this(
		id: GenerateId(planId, userId),
		planId,
		userId,
		joinedAt: DateTime.Today,
		leftAt: null,
		statusId: PlanParticipantStatus.Active.Id)
	{
	}

	public static Guid GenerateId(Guid planId, Guid userId)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		return planId.Combine(userId);
	}

	public static PlanParticipant CreateNew(Guid planId, Guid userId)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");
		
		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		return new PlanParticipant(planId, userId);
	}

	public static PlanParticipant Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime joinedAt,
		DateTime? leftAt,
		int statusId)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), "Informe o ID do participante do plano");

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		if (joinedAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(joinedAt), "Informe a data de inclusão do usuário no plano");

		if (statusId <= 0)
			throw new ArgumentException("O ID do status do participante deve ser maior que zero", nameof(statusId));

		return new PlanParticipant(
			id,
			planId,
			userId,
			joinedAt,
			leftAt,
			statusId);
	}
}
