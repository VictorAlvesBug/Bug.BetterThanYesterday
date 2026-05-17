using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;

// TODO: Ajustar futuramente para evitar repetição de status entre CheckInStatus e ReviewStatus
public sealed class CheckInStatus : Enumeration<CheckInStatus>
{
	public static readonly CheckInStatus Pending = new(1, nameof(Pending));
	public static readonly CheckInStatus Validated = new(2, nameof(Validated));
	public static readonly CheckInStatus Rejected = new(3, nameof(Rejected));

	private CheckInStatus(int id, string name) : base(id, name) { }

	public bool IsValidAsReviewStatus() => this == Validated || this == Rejected;
}
