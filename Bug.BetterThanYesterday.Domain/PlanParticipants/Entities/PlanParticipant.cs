using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.Entities
{
	public class PlanParticipant : Entity
	{
		public string PlanId { get; set; }
		public string UserId { get; set; }
		public DateOnly JoinedAt { get; set; }
		public DateOnly? LeftAt { get; set; }
		public PlanParticipantStatus Status { get; set; }

		private PlanParticipant(string planId, string userId)
		{
			Id = Guid.NewGuid().ToString();
			PlanId = planId;
			UserId = userId;
			JoinedAt = DateOnly.FromDateTime(DateTime.Today);
			Status = PlanParticipantStatus.Active;
		}

		public static PlanParticipant CreateNew(string planId, string userId)
		{
			if (string.IsNullOrWhiteSpace(planId))
				throw new ArgumentNullException(nameof(planId), "Informe o ID do plano");
			
			if (string.IsNullOrWhiteSpace(userId))
				throw new ArgumentNullException(nameof(userId), "Informe o ID do usuário");

			return new PlanParticipant(planId, userId);
		}
	}
}
