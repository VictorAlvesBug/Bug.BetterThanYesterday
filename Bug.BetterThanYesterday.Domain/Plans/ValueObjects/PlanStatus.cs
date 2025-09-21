using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

public sealed class PlanStatus : Enumeration
{
	public static readonly PlanStatus NotStarted = new(1, nameof(NotStarted));
	public static readonly PlanStatus Running = new(2, nameof(Running));
	public static readonly PlanStatus Finished = new(3, nameof(Finished));
	public static readonly PlanStatus Cancelled = new(4, nameof(Cancelled));

	private PlanStatus(int id, string name) : base(id, name) { }

	public static PlanStatus FromId(int id) =>
		GetAll<PlanStatus>().FirstOrDefault(type => type.Id == id)
		?? throw new ArgumentOutOfRangeException(nameof(id), $"ID do PlanStatus inválido: {id}");
	public static PlanStatus FromName(string name) =>
		GetAll<PlanStatus>().FirstOrDefault(type => type.Name == name)
		?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome do PlanStatus inválido: {name}");
}
