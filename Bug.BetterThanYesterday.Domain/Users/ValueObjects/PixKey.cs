using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Strings;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed record PixKey
{
	public string Value { get; }
	private PixKeyType PixKeyType { get; }

	private PixKey(string value, PixKeyType pixKeyType)
	{
		Value = value;
		PixKeyType = pixKeyType;
	}

	public static PixKey Create(string value, string pixKeyTypeIdOrName)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentNullException(nameof(PixKey), Messages.EnterUserPixKey);

		value = value.Trim().ToLower();

		var pixKeyType = PixKeyType.Get(pixKeyTypeIdOrName);

		if (pixKeyType == PixKeyType.TaxIdentification)
		{
			if(TaxIdentification.TryCreate(value, out var _, out var TaxIdErrorMessage))
				return new PixKey(value.OnlyDigits(), pixKeyType);

			throw new ArgumentException(TaxIdErrorMessage, nameof(PixKey));
		}

		if (pixKeyType == PixKeyType.Email)
		{
			if(Email.TryCreate(value, out var _, out var emailErrorMessage))
				return new PixKey(value, pixKeyType);

			throw new ArgumentException(emailErrorMessage, nameof(PixKey));
		}

		if (pixKeyType == PixKeyType.PhoneNumber)
		{
			if(PhoneNumber.TryCreate(value, out var _, out var phoneNumberErrorMessage))
				return new PixKey(value.OnlyDigits(), pixKeyType);

			throw new ArgumentException(phoneNumberErrorMessage, nameof(PixKey));
		}

		if (pixKeyType == PixKeyType.EVP)
		{
			if(EVP.TryCreate(value, out var _, out var evpErrorMessage))
				return new PixKey(value, pixKeyType);

			throw new ArgumentException(evpErrorMessage, nameof(PixKey));
		}
			
		throw new ArgumentException(Messages.EnterValidUserPixKey, nameof(PixKey));
	}

	public override string ToString() => Value;

	public string GetPixKeyType()
	{
		return PixKeyType.Name;
	}
}
