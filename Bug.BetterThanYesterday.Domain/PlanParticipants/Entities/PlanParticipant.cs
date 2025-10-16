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
		id: BuildId(planId, userId),
		planId,
		userId,
		joinedAt: DateTime.Today,
		leftAt: null,
		statusId: PlanParticipantStatus.Active.Id)
	{
	}

	public static Guid BuildId(Guid planId, Guid userId)
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

	public void MarkAsLeft()
	{
		if (Status == PlanParticipantStatus.Active)
		{
			Status = PlanParticipantStatus.Left;
			LeftAt = DateOnly.FromDateTime(DateTime.Today);
			return;
		}

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException("Este usuário já foi removido do plano");

		if (Status == PlanParticipantStatus.Blocked)
			throw new InvalidOperationException("Este usuário não pode ser removido do plano, pois está bloqueado");
			
		throw new InvalidOperationException("Alteração não mapeada para status atual");
    }

	public void MarkAsBlocked()
	{
		if (Status == PlanParticipantStatus.Active)
		{
			Status = PlanParticipantStatus.Blocked;
			return;
		}

        if (Status == PlanParticipantStatus.Blocked)
			throw new InvalidOperationException("Este participante já está bloqueado neste plano");

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException("O usuário não está mais neste plano");
			
		throw new InvalidOperationException("Alteração não mapeada para status atual");
    }

	public void MarkAsActive()
	{
		if (Status == PlanParticipantStatus.Blocked)
		{
			Status = PlanParticipantStatus.Active;
			return;
		}

        if (Status == PlanParticipantStatus.Active)
			throw new InvalidOperationException("Este participante já está desbloqueado neste plano");

		if (Status == PlanParticipantStatus.Left)
			throw new InvalidOperationException("O usuário não está mais neste plano");
			
		throw new InvalidOperationException("Alteração não mapeada para status atual");
    }
}
