using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Strings;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed record PhoneNumber
{
	public string Value { get; }

	private PhoneNumber(string value)
	{
		Value = value;
	}

	public static PhoneNumber Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentNullException(nameof(PhoneNumber), Messages.EnterUserPhoneNumber);

		var regex = new Regex(@"^\d{2}(9\d{4}|\d{4})\d{4}$");

		value = value.Trim().ToLower().OnlyDigits();

		if (!regex.IsMatch(value))
			throw new ArgumentException(nameof(PhoneNumber), Messages.EnterValidUserPhoneNumber);

		return new PhoneNumber(value);
	}

	public override string ToString() => Value;
}
