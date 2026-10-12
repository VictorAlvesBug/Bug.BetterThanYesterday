namespace Bug.BetterThanYesterday.Application.CheckIns.ReplaceCheckIn;

public class ReplaceCheckInRequest
{
	public required Guid UserId { get; init; }
	public required string Title { get; init; }
	public required string EvidencePhotoUrl { get; init; }
}
