using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

public sealed class PlanStatus : Enumeration<PlanStatus>
{
	public static readonly PlanStatus NotStarted = new(1, nameof(NotStarted));
	public static readonly PlanStatus Running = new(2, nameof(Running));
	public static readonly PlanStatus Finished = new(3, nameof(Finished));
	public static readonly PlanStatus Cancelled = new(4, nameof(Cancelled));

	private PlanStatus(int id, string name) : base(id, name) { }
}
