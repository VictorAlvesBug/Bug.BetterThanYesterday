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
		if (TryCreate(value, out PhoneNumber? phoneNumber, out string? errorMessage))
			return phoneNumber!;

		throw new ArgumentException(errorMessage, nameof(PhoneNumber));
	}

	public static bool TryCreate(string value, out PhoneNumber? phoneNumber, out string? errorMessage)
	{
		phoneNumber = null;
		errorMessage = null;

		if (string.IsNullOrWhiteSpace(value))
		{
			errorMessage = Messages.EnterUserPhoneNumber;
			return false;
		}

		var regex = new Regex(@"^\d{2}(9\d{4}|\d{4})\d{4}$");

		value = value.Trim().ToLower().OnlyDigits();

		if (!regex.IsMatch(value)){
			errorMessage = Messages.EnterValidUserPhoneNumber;
			return false;
}
		phoneNumber = new PhoneNumber(value);
		return true;
	}

	public override string ToString() => Value;
}
