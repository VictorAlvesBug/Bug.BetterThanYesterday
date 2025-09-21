using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Plans.Entities;

public class Plan : Entity
{
	public string HabitId { get; set; }
	public string? Description { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public PlanStatus Status { get; set; }
	public PlanType Type { get; set; }
	public DateOnly CreatedAt { get; set; }

	private Dictionary<PlanStatus, List<PlanStatus>> _allowedStatusChanges = new()
	{
		{ PlanStatus.Draft, new List<PlanStatus> { PlanStatus.Running, PlanStatus.Cancelled } },
		{ PlanStatus.Running, new List<PlanStatus> { PlanStatus.Finished, PlanStatus.Cancelled } },
		{ PlanStatus.Finished, new List<PlanStatus>() },
		{ PlanStatus.Cancelled, new List<PlanStatus>() }
	};

	private Plan(
		string id,
		string habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int status,
		int type,
		DateTime createdAt)
	{
		Id = id;
		HabitId = habitId;
		Description = description;
		StartsAt = DateOnly.FromDateTime(startsAt);
		EndsAt = DateOnly.FromDateTime(endsAt);
		Status = PlanStatus.FromId(status);
		Type = PlanType.FromId(type);
		CreatedAt = DateOnly.FromDateTime(createdAt);
	}

	private Plan(
		string habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int type)
	: this(
		id: Guid.NewGuid().ToString(),
		habitId,
		description,
		startsAt,
		endsAt,
		status: PlanStatus.Draft.Id,
		type,
		createdAt: DateTime.Today)
	{
	}

	public static Plan CreateNew(
		string habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int type)
	{
		if (string.IsNullOrWhiteSpace(habitId))
			throw new ArgumentNullException(nameof(habitId), "Informe o ID do hábito");
		
		if (endsAt <= startsAt)
			throw new ArgumentException("A data de término deve ser após a data de início");

		return new Plan(habitId, description, startsAt, endsAt, type);
	}

	public static Plan Restore(
		string id,
		string habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int status,
		int type,
		DateTime createdAt)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentNullException(nameof(id), "Informe o ID do plano");

		if (string.IsNullOrWhiteSpace(habitId))
			throw new ArgumentNullException(nameof(habitId), "Informe o ID do hábito");

		if (endsAt <= startsAt)
			throw new ArgumentException("A data de término deve ser após a data de início");

		return new Plan(
			id,
			habitId,
			description,
			startsAt,
			endsAt,
			status,
			type,
			createdAt);
	}

	public void ChangeStatus(PlanStatus newStatus)
	{
		var isChangeAllowed = _allowedStatusChanges.ContainsKey(Status)
			&& _allowedStatusChanges[Status].Contains(newStatus);

		if (!isChangeAllowed)
			throw new InvalidOperationException($"Não é possível alterar o status de {Status.Name} para {newStatus.Name}");

		Status = newStatus;
	}
}
