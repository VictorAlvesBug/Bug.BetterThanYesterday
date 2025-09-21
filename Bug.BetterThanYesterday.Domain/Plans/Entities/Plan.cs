using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Plans.Entities;

public sealed class Plan : Entity
{
	public Guid HabitId { get; set; }
	public string? Description { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public PlanStatus Status { get; set; }
	public PlanType Type { get; set; }
	public DateOnly CreatedAt { get; set; }

	private Dictionary<PlanStatus, List<PlanStatus>> _allowedStatusChanges = new()
	{
		{ PlanStatus.NotStarted, new List<PlanStatus> { PlanStatus.Running, PlanStatus.Cancelled } },
		{ PlanStatus.Running, new List<PlanStatus> { PlanStatus.Finished, PlanStatus.Cancelled } },
		{ PlanStatus.Finished, new List<PlanStatus>() },
		{ PlanStatus.Cancelled, new List<PlanStatus>() }
	};

	private Plan(
		Guid id,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int statusId,
		int typeId,
		DateTime createdAt)
	{
		Id = id;
		HabitId = habitId;
		Description = description;
		StartsAt = DateOnly.FromDateTime(startsAt);
		EndsAt = DateOnly.FromDateTime(endsAt);
		Status = PlanStatus.FromId(statusId);
		Type = PlanType.FromId(typeId);
		CreatedAt = DateOnly.FromDateTime(createdAt);
	}

	private Plan(
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int typeId)
	: this(
		id: Guid.NewGuid(),
		habitId,
		description,
		startsAt,
		endsAt,
		statusId: PlanStatus.NotStarted.Id,
		typeId,
		createdAt: DateTime.Today)
	{
	}

	public static Plan CreateNew(
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int typeId)
	{
		if (habitId == Guid.Empty)
			throw new ArgumentNullException(nameof(habitId), "Informe o ID do hábito");

		if (startsAt < DateTime.Today)
			throw new ArgumentException("A data de início não pode ser anterior a hoje", nameof(startsAt));

		if (endsAt <= startsAt)
			throw new ArgumentException("A data de término deve ser após a data de início", nameof(endsAt));

		if (typeId <= 0)
			throw new ArgumentException("O ID do tipo de plano deve ser maior que zero", nameof(typeId));

		return new Plan(habitId, description, startsAt, endsAt, typeId);
	}

	public static Plan Restore(
		Guid id,
		Guid habitId,
		string? description,
		DateTime startsAt,
		DateTime endsAt,
		int statusId,
		int typeId,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), "Informe o ID do plano");

		if (habitId == Guid.Empty)
			throw new ArgumentNullException(nameof(habitId), "Informe o ID do hábito");

		if (startsAt <= DateTime.MinValue)
			throw new ArgumentException("Data de início do plano inválida", nameof(startsAt));

		if (endsAt <= startsAt)
			throw new ArgumentException("A data de término deve ser após a data de início", nameof(endsAt));

		if (statusId <= 0)
			throw new ArgumentException("O ID do status do plano deve ser maior que zero", nameof(statusId));

		if (typeId <= 0)
			throw new ArgumentException("O ID do tipo de plano deve ser maior que zero", nameof(typeId));

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), "Informe a data de criação do plano");

		return new Plan(
			id,
			habitId,
			description,
			startsAt,
			endsAt,
			statusId,
			typeId,
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
