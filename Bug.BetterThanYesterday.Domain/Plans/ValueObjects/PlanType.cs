using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects;

public sealed class PlanType : Enumeration
{
	public static readonly PlanType Public = new(1, nameof(Public));
	public static readonly PlanType Private = new(2, nameof(Private));

	private PlanType(int id, string name) : base(id, name) { }

	public static PlanType FromId(int id) =>
		GetAll<PlanType>().FirstOrDefault(type => type.Id == id)
		?? throw new ArgumentOutOfRangeException(nameof(id), $"ID do PlanType inválido: {id}");
	public static PlanType FromName(string name) =>
		GetAll<PlanType>().FirstOrDefault(type => type.Name == name)
		?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome do PlanType inválido: {name}");
}
