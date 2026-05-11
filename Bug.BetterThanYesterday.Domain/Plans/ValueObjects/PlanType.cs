using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

public sealed class PlanType : Enumeration<PlanType>
{
	public static readonly PlanType Public = new(1, nameof(Public));
	public static readonly PlanType Private = new(2, nameof(Private));

	private PlanType(int id, string name) : base(id, name) { }
}
