namespace Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;

public sealed record PlanParticipantStatus
{
	public static readonly PlanParticipantStatus Active = new(nameof(Active));
	public static readonly PlanParticipantStatus Left = new(nameof(Left));
	public static readonly PlanParticipantStatus Blocked = new(nameof(Blocked));
	public string Value { get; }
	private PlanParticipantStatus(string value)
	{
		Value = value;
	}
	public override string ToString() => Value;

	public static implicit operator string(PlanParticipantStatus status) => status.Value;

	public static explicit operator PlanParticipantStatus(string value) => value switch
	{
		nameof(Active) => Active,
		nameof(Left) => Left,
		nameof(Blocked) => Blocked,
		_ => throw new InvalidCastException($"Erro ao converter '{value}' para um PlanParticipantStatus.")
	};
}
