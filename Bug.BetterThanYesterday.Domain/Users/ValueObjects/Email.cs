using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed record Email
{
	public string Value { get; }

	private Email(string value)
	{
		Value = value;
	}

	public static Email Create(string value)
	{
		if(TryCreate(value, out Email? email, out string? errorMessage))
			return email!;

		throw new ArgumentException(errorMessage, nameof(Email));
	}

	public static bool TryCreate(string value, out Email? email, out string? errorMessage)
	{
		email = null;
		errorMessage = null;

		if (string.IsNullOrWhiteSpace(value))
		{
			errorMessage = Messages.EnterUserEmail;
			return false;
		}

		var regex = new Regex(@"^[a-z0-9+_.-]+@([a-z0-9-]+\.)+[a-z]{2,6}$");

		value = value.Trim().ToLower();

		if (!regex.IsMatch(value))
		{	
			errorMessage = Messages.EnterValidUserEmail;
			return false;
		}

		email = new Email(value);
		return true;
	}

	public override string ToString() => Value;
}
