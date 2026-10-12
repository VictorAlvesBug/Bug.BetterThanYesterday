using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Domain.Reports.Entities;

public sealed class CheckInReport : Entity
{
	public Guid CheckInId { get; set; }
	public Guid PlanId { get; set; }
	public Guid ReporterId { get; set; }
	public Guid TargetUserId { get; set; }
	public string Reason { get; set; } = string.Empty;

	public static CheckInReport CreateNew(
		Guid checkInId,
		Guid planId,
		Guid reporterId,
		Guid targetUserId,
		string reason)
	{
		if (checkInId == Guid.Empty)
			throw new ArgumentNullException(nameof(checkInId), Messages.EnterCheckInId);

		if (planId == Guid.Empty)
			throw new ArgumentNullException(nameof(planId), Messages.EnterPlanId);

		if (reporterId == Guid.Empty)
			throw new ArgumentNullException(nameof(reporterId), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(reason))
			throw new ArgumentException(Messages.EnterReportReason, nameof(reason));

		return new CheckInReport
		{
			Id = Guid.NewGuid(),
			CheckInId = checkInId,
			PlanId = planId,
			ReporterId = reporterId,
			TargetUserId = targetUserId,
			Reason = reason.Trim(),
			CreatedAt = DateTime.Now
		};
	}

	public static CheckInReport Restore(
		Guid id,
		Guid checkInId,
		Guid planId,
		Guid reporterId,
		Guid targetUserId,
		string reason,
		DateTime createdAt) => new()
	{
		Id = id,
		CheckInId = checkInId,
		PlanId = planId,
		ReporterId = reporterId,
		TargetUserId = targetUserId,
		Reason = reason,
		CreatedAt = createdAt
	};
}
