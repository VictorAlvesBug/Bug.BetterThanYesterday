using Bug.BetterThanYesterday.Domain.Strings;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed record TaxIdentification
{
	public string Value { get; }

	private TaxIdentification(string value)
	{
		Value = value;
	}

	public static bool TryCreate(string value, out TaxIdentification? taxIdentification, out string? errorMessage)
	{
		taxIdentification = null;
		errorMessage = null;

		if (value.Length == 11)
		{
			var cpfRegex = new Regex(@"^[0-9]{11}$");

			if (!cpfRegex.IsMatch(value))
			{
				errorMessage = Messages.EnterValidCpfTaxIdentification;
				return false;
			}

			taxIdentification = new TaxIdentification(value);
			return true;
		}


		if (value.Length == 14)
		{
			var cnpjRegex = new Regex(@"^[0-9]{11}$");

			if (!cnpjRegex.IsMatch(value))
			{
				errorMessage = Messages.EnterValidCnpjTaxIdentification;
				return false;
			}

			taxIdentification = new TaxIdentification(value);
			return true;
		}

		errorMessage = Messages.EnterValidTaxIdentification;
		return false;
	}

	public override string ToString() => Value;
}
