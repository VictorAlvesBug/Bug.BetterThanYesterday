using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;

public sealed class PlanMemberStatus : Enumeration
{
	public static readonly PlanMemberStatus Active = new(1, nameof(Active));
	public static readonly PlanMemberStatus Left = new(2, nameof(Left));
	public static readonly PlanMemberStatus Blocked = new(3, nameof(Blocked));
	private PlanMemberStatus(int id, string name) : base(id, name) { }

	public static PlanMemberStatus FromId(int id) =>
		GetAll<PlanMemberStatus>().FirstOrDefault(type => type.Id == id)
		?? throw new ArgumentOutOfRangeException(nameof(id), $"ID do PlanMemberStatus inválido: {id}");
	public static PlanMemberStatus FromName(string name) =>
		GetAll<PlanMemberStatus>().FirstOrDefault(type => type.Name == name)
		?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome do PlanMemberStatus inválido: {name}");
}
