using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserCommand : ICommand
{
	public UpdateUserCommand(
		Guid userId,
		string? name = null,
		string? email = null,
		string? photoUrl = null,
		string? nickname = null,
		string? phoneNumber = null,
		string? pixKey = null,
		string? pixKeyType = null)
	{
		UserId = userId;
		Name = name;
		Email = email;
		PhotoUrl = photoUrl;
		Nickname = nickname;
		PhoneNumber = phoneNumber;
		PixKey = pixKey;
		PixKeyType = pixKeyType;
	}

	public Guid UserId { get; init; }
	public string? Name { get; init; }
	public string? Email { get; init; }
	public string? PhotoUrl { get; init; }
	public string? Nickname { get; init; }
	public string? PhoneNumber { get; init; }
	public string? PixKey { get; init; }
	public string? PixKeyType { get; init; }

	public void Validate()
	{
		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

		var hasAnyField =
			!string.IsNullOrWhiteSpace(Name) ||
			!string.IsNullOrWhiteSpace(Email) ||
			!string.IsNullOrWhiteSpace(PhotoUrl) ||
			!string.IsNullOrWhiteSpace(Nickname) ||
			!string.IsNullOrWhiteSpace(PhoneNumber) ||
			!string.IsNullOrWhiteSpace(PixKey) ||
			!string.IsNullOrWhiteSpace(PixKeyType);

		if (!hasAnyField)
			throw new ArgumentException(Messages.EnterUserNameOrEmail);
	}
}
