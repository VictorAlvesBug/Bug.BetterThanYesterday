using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Plans.Entities
{
	public class Plan : Entity
	{
		public string HabitId { get; set; }
		public string? Description { get; set; }
		public DateOnly StartsAt { get; set; }
		public DateOnly EndsAt { get; set; }
		public PlanStatus Status { get; set; }
		public PlanType Type { get; set; }
		public DateOnly CreatedAt { get; set; }

		private Plan(
			string habitId,
			string? description,
			DateOnly startsAt,
			DateOnly endsAt,
			PlanType type)
		{
			Id = Guid.NewGuid().ToString();
			HabitId = habitId;
			Description = description;
			StartsAt = startsAt;
			EndsAt = endsAt;
			Status = PlanStatus.Draft;
			Type = type;
			CreatedAt = DateOnly.FromDateTime(DateTime.Today);
		}

		public static Plan CreateNew(
			string habitId,
			string? description,
			DateOnly startsAt,
			DateOnly endsAt,
			string type)
		{
			if (string.IsNullOrWhiteSpace(habitId))
				throw new ArgumentNullException(nameof(habitId), "Informe o ID do hábito");
			
			if (endsAt <= startsAt)
				throw new ArgumentException("A data de término deve ser após a data de início");

			return new Plan(habitId, description, startsAt, endsAt, (PlanType) type);
		}
	}
}
