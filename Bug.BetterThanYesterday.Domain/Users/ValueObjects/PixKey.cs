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

	public static PixKey Create(string value, string pixKeyTypeName)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentNullException(nameof(PixKey), Messages.EnterUserPixKey);

		value = value.Trim().ToLower().OnlyDigits();

		var pixKeyType = PixKeyType.FromName(pixKeyTypeName);

		if (pixKeyType == PixKeyType.TaxIdentification)
		{
			TaxIdentification.Create(value);
			return new PixKey(value, pixKeyType);
		}

		if (pixKeyType == PixKeyType.Email)
		{
			Email.Create(value);
			return new PixKey(value, pixKeyType);
		}

		if (pixKeyType == PixKeyType.PhoneNumber)
		{
			PhoneNumber.Create(value);
			return new PixKey(value, pixKeyType);
		}

		if (pixKeyType == PixKeyType.EVP)
		{
			EVP.Create(value);
			return new PixKey(value, pixKeyType);
		}
			
		throw new ArgumentException(nameof(PixKey), Messages.EnterValidUserPixKey);
	}

	public override string ToString() => Value;

	public string GetPixKeyType()
	{
		return PixKeyType.Name;
	}
}
