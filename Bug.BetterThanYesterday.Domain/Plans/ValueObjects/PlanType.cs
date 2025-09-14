namespace Bug.BetterThanYesterday.Domain.Plans.ValueObjects
{
	public sealed record PlanType
	{
		public static readonly PlanType Public = new(nameof(Public));
		public static readonly PlanType Private = new(nameof(Private));
		public string Value { get; }
		private PlanType(string value)
		{
			Value = value;
		}
		public override string ToString() => Value;

		public static implicit operator string(PlanType type) => type.Value;

		public static explicit operator PlanType(string value) => value switch
		{
			nameof(Public) => Public,
			nameof(Private) => Private,
			_ => throw new InvalidCastException($"Erro ao converter '{value}' para um PlanType.")
		};
	}
}
