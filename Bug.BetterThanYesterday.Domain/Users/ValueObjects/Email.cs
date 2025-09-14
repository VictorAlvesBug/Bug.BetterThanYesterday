using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects
{
	public sealed record Email
	{
		public string Value { get; }

		private Email(string value)
		{
			Value = value;
		}

		public static Email Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(nameof(Email), "Informe o e-mail do usuário");

			var regex = new Regex(@"^[a-z0-9+_.-]+@([a-z0-9-]+\.)+[a-z]{2,6}$");

			value = value.Trim().ToLower();

			if (!regex.IsMatch(value))
				throw new ArgumentException(nameof(Email), "Informe um e-mail válido para o usuário");

			return new Email(value);
		}

		public override string ToString() => Value;
	}
}
