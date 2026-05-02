using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserCommand : ICommand
{
	public RegisterUserCommand(
		string name,
		string email,
		string? photoUrl,
		string nickname,
		string phoneNumber,
		string pixKey,
		string pixKeyType)
	{
		Name = name;
		Email = email;
		PhotoUrl = photoUrl;
		Nickname = nickname;
		PhoneNumber = phoneNumber;
		PixKey = pixKey;
		PixKeyType = pixKeyType;
	}

	public string Name { get; init; }
	public string Email { get; init; }
	public string? PhotoUrl { get; init; }
	public string Nickname { get; init; }
	public string PhoneNumber { get; init; }
	public string PixKey { get; init; }
	public string PixKeyType { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), Messages.EnterUserEmail);

		if (string.IsNullOrWhiteSpace(Nickname))
			throw new ArgumentNullException(nameof(Nickname), Messages.EnterUserNickname);

		if (string.IsNullOrWhiteSpace(PhoneNumber))
			throw new ArgumentNullException(nameof(PhoneNumber), Messages.EnterUserPhoneNumber);

		if (string.IsNullOrWhiteSpace(PixKey))
			throw new ArgumentNullException(nameof(PixKey), Messages.EnterUserPixKey);

		if (string.IsNullOrWhiteSpace(PixKeyType))
			throw new ArgumentNullException(nameof(PixKeyType), Messages.EnterUserPixKeyType);
	}
}
