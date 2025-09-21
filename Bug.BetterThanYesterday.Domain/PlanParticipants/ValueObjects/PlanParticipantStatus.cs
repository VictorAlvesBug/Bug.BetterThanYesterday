using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

public sealed class PlanParticipantStatus : Enumeration
{
	public static readonly PlanParticipantStatus Active = new(1, nameof(Active));
	public static readonly PlanParticipantStatus Left = new(2, nameof(Left));
	public static readonly PlanParticipantStatus Blocked = new(3, nameof(Blocked));
	private PlanParticipantStatus(int id, string name) : base(id, name) { }

	public static PlanParticipantStatus FromId(int id) =>
		GetAll<PlanParticipantStatus>().FirstOrDefault(type => type.Id == id)
		?? throw new ArgumentOutOfRangeException(nameof(id), $"ID do PlanParticipantStatus inválido: {id}");
	public static PlanParticipantStatus FromName(string name) =>
		GetAll<PlanParticipantStatus>().FirstOrDefault(type => type.Name == name)
		?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome do PlanParticipantStatus inválido: {name}");
}
