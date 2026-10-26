using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;

public class ListCheckInReportsCommand : ICommand
{
	public required Guid ChallengeId { get; init; }
	public required Guid RequesterId { get; init; }
	public bool IsPlatformAdmin { get; init; }

	public void Validate()
	{
		if (ChallengeId == Guid.Empty)
			throw new ArgumentNullException(nameof(ChallengeId), Messages.EnterChallengeId);

		if (RequesterId == Guid.Empty)
			throw new ArgumentNullException(nameof(RequesterId), Messages.EnterUserId);
	}
}
