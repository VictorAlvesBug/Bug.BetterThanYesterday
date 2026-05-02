using Bug.BetterThanYesterday.Domain.Strings;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Common.ValueObjects;

public sealed record Photo
{
	public string Value { get; }

	private Photo(string value)
	{
		Value = value;
	}

	public static Photo Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentNullException(nameof(Photo), Messages.EnterPhoto);

		var regex = new Regex(@"^https?:\/\/s3.[a-z0-9\-]+\.amazonaws.com(\/[a-z0-9\-_]*)*\.(jpg|jpeg|png)$");

		value = value.Trim().ToLower();

		if (!regex.IsMatch(value))
			throw new ArgumentException(nameof(Photo), Messages.EnterValidPhoto);

		return new Photo(value);
	}

	public override string ToString() => Value;
}
