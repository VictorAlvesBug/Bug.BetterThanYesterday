using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Extensions;

namespace Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;

public sealed class PlanMemberStatus : Enumeration<PlanMemberStatus>
{
	public static readonly PlanMemberStatus Active = new(1, nameof(Active));
	public static readonly PlanMemberStatus Blocked = new(2, nameof(Blocked));
	private PlanMemberStatus(int id, string name) : base(id, name) { }
}
