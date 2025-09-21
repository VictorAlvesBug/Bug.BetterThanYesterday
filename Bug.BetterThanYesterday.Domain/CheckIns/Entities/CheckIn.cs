using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns.Entities;

public class CheckIn : Entity
{
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateOnly Date { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }

	private CheckIn(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		string title,
		string? description)
	{
		Id = id;
		PlanId = planId;
		UserId = userId;
		Date = DateOnly.FromDateTime(date);
		Title = title;
		Description = description;
	}

	private CheckIn(
		Guid planId,
		Guid userId,
		DateTime date,
		string title,
		string? description)
	: this(
			id: Guid.NewGuid(),
			planId,
			userId,
			date,
			title,
			description)
	{ }

	public static CheckIn CreateNew(
		Guid planId,
		Guid userId,
		DateTime date,
		string title,
		string? description)
	{
		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), "Informe a data do check-in");

		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentNullException(nameof(title), "Informe o título do check-in");

		return new CheckIn(planId, userId, date, title, description);
	}

	public static CheckIn Restore(
		Guid id,
		Guid planId,
		Guid userId,
		DateTime date,
		string title,
		string? description)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), "Informe o ID do check-in");

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

		if (userId == Guid.Empty)
			throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

		if (date == DateTime.MinValue)
			throw new ArgumentNullException(nameof(date), "Informe a data do check-in");

		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentNullException(nameof(title), "Informe o título do check-in");

		return new CheckIn(
			id,
			planId,
			userId,
			date,
			title,
			description);
	}
}
