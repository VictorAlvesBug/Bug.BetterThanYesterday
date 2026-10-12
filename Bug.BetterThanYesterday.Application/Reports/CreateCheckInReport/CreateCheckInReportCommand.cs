using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport;

public class CreateCheckInReportCommand : ICommand
{
	public required Guid CheckInId { get; init; }
	public required Guid ReporterId { get; init; }
	public required string Reason { get; init; }

	public void Validate()
	{
		if (CheckInId == Guid.Empty)
			throw new ArgumentNullException(nameof(CheckInId), Messages.EnterCheckInId);

		if (ReporterId == Guid.Empty)
			throw new ArgumentNullException(nameof(ReporterId), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(Reason))
			throw new ArgumentException(Messages.EnterReportReason, nameof(Reason));
	}
}
