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

	public static TaxIdentification Create(string value)
	{
		if (value.Length == 11)
		{
			var cpfRegex = new Regex(@"^[0-9]{11}$");

			if (!cpfRegex.IsMatch(value))
				throw new ArgumentException(nameof(TaxIdentification), Messages.EnterValidCpfTaxIdentification);

			return new TaxIdentification(value);
		}


		if (value.Length == 14)
		{
			var cnpjRegex = new Regex(@"^[0-9]{11}$");

			if (!cnpjRegex.IsMatch(value))
				throw new ArgumentException(nameof(TaxIdentification), Messages.EnterValidCnpjTaxIdentification);

			return new TaxIdentification(value);
		}
		
		throw new ArgumentException(nameof(TaxIdentification), Messages.EnterValidTaxIdentification);

	}

	public override string ToString() => Value;
}
