namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects
{
	public sealed record PlanStatus
	{
		public static readonly PlanStatus Draft = new(nameof(Draft));
		public static readonly PlanStatus Running = new(nameof(Running));
		public static readonly PlanStatus Finished = new(nameof(Finished));
		public static readonly PlanStatus Cancelled = new(nameof(Cancelled));
		public string Value { get; }
		private PlanStatus(string value)
		{
			Value = value;
		}
		public override string ToString() => Value;
		
		public static implicit operator string(PlanStatus status) => status.Value;
		
		public static explicit operator PlanStatus(string value) => value switch
		{
			nameof(Draft) => Draft,
			nameof(Running) => Running,
			nameof(Finished) => Finished,
			nameof(Cancelled) => Cancelled,
			_ => throw new InvalidCastException($"Erro ao converter '{value}' para um PlanStatus.")
		};
	}
}
