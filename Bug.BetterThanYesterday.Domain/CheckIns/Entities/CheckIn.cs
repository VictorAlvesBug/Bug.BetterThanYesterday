using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns.Entities
{
	public class CheckIn : Entity
	{
		public string PlanId { get; set; }
		public string UserId { get; set; }
		public DateOnly Date { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }

		private CheckIn(
			string id,
			string planId,
			string userId,
			DateOnly date,
			string title,
			string? description)
		{
			Id = id;
			PlanId = planId;
			UserId = userId;
			Date = date;
			Title = title;
			Description = description;
		}

		private CheckIn(
			string planId,
			string userId,
			DateOnly date,
			string title,
			string? description)
		: this(
				id: Guid.NewGuid().ToString(),
				planId,
				userId,
				date,
				title,
				description)
		{ }

		public static CheckIn CreateNew(
			string planId,
			string userId,
			DateOnly date,
			string title,
			string? description)
		{
			if (string.IsNullOrWhiteSpace(planId))
				throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

			if (string.IsNullOrWhiteSpace(userId))
				throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentNullException(nameof(title), "Informe o título do check-in");

			return new CheckIn(planId, userId, date, title, description);
		}

		public static CheckIn Restore(
			string id,
			string planId,
			string userId,
			DateOnly date,
			string title,
			string? description)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentNullException(nameof(id), "Informe o ID do check-in");

			if (string.IsNullOrWhiteSpace(planId))
				throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");

			if (string.IsNullOrWhiteSpace(userId))
				throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

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
}
