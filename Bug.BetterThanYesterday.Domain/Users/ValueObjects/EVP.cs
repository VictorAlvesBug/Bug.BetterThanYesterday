using Bug.BetterThanYesterday.Domain.Strings;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed record EVP
{
	public string Value { get; }

	private EVP(string value)
	{
		Value = value;
	}

	public static EVP Create(string value)
	{
		var regex = new Regex(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$");

			if (!regex.IsMatch(value))
				throw new ArgumentException(nameof(EVP), Messages.EnterValidEVP);

			return new EVP(value);

	}

	public override string ToString() => Value;
}
