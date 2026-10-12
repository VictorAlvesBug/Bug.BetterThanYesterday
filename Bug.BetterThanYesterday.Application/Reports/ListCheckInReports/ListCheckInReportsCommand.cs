using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;

public class ListCheckInReportsCommand : ICommand
{
	public required Guid PlanId { get; init; }
	public required Guid RequesterId { get; init; }
	public bool IsPlatformAdmin { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);

		if (RequesterId == Guid.Empty)
			throw new ArgumentNullException(nameof(RequesterId), Messages.EnterUserId);
	}
}
