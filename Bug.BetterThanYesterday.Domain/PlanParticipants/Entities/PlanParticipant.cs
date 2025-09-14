using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;

public class PlanParticipant : Entity
{
	public string PlanId { get; set; }
	public string UserId { get; set; }
	public DateOnly JoinedAt { get; set; }
	public DateOnly? LeftAt { get; set; }
	public PlanParticipantStatus Status { get; set; }

	private PlanParticipant(
		string id,
		string planId,
		string userId,
		DateOnly joinedAt,
		DateOnly? leftAt,
		PlanParticipantStatus status)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		JoinedAt = joinedAt;
		LeftAt = leftAt;
		Status = status;
	}

	private PlanParticipant(string planId, string userId)
		: this(
		id: Guid.NewGuid().ToString(),
		planId,
		userId,
		joinedAt: DateOnly.FromDateTime(DateTime.Today),
		leftAt: null,
		status: PlanParticipantStatus.Active)
	{
	}

	public static PlanParticipant CreateNew(string planId, string userId)
	{
		if (string.IsNullOrWhiteSpace(planId))
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");
		
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		return new PlanParticipant(planId, userId);
	}

	public static PlanParticipant Restore(
		string id,
		string planId,
		string userId,
		DateOnly joinedAt,
		DateOnly? leftAt,
		PlanParticipantStatus status)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentNullException(nameof(id), "Informe o ID do participante do plano");

		if (string.IsNullOrWhiteSpace(planId))
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		return new PlanParticipant(
			id,
			planId,
			userId,
			joinedAt,
			leftAt,
			status);
	}
}
