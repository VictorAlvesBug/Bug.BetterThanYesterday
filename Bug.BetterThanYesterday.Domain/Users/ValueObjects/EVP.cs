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
		if (TryCreate(value, out EVP? evp, out string? errorMessage))
			return evp!;

		throw new ArgumentException(errorMessage, nameof(EVP));
	}

	public static bool TryCreate(string value, out EVP? evp, out string? errorMessage)
	{
		evp = null;
		errorMessage = null;

		var regex = new Regex(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$");

		if (!regex.IsMatch(value))
		{
			errorMessage = Messages.EnterValidEVP;
			return false;
		}
		evp = new EVP(value);
		return true;

	}

	public override string ToString() => Value;
}
