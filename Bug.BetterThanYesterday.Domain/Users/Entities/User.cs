using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Common.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Users.Entities;

public class User : Entity
{
	public string Name { get; set; }
	public Email Email { get; set; }
	public Photo? Photo { get; set; }
	public string Nickname { get; set; }
	public PhoneNumber PhoneNumber { get; set; }
	public PixKey PixKey { get; set; }

	private User(
		Guid id,
		string name,
		string email,
		string? photoUrl,
		string nickname,
		string phoneNumber,
		string pixKey,
		string pixKeyType,
		DateTime createdAt)
	{
		Id = id;
		Name = name;
		Email = Email.Create(email);
		Photo = photoUrl is null ? null : Photo.Create(photoUrl);
		Nickname = nickname;
		PhoneNumber = PhoneNumber.Create(phoneNumber);
		PixKey = PixKey.Create(pixKey, pixKeyType);
		CreatedAt = DateOnly.FromDateTime(createdAt);
	}

	private User(
		string name,
		string email,
		string? photoUrl,
		string nickname,
		string phoneNumber,
		string pixKey,
		string pixKeyType)
		: this(
			id: Guid.NewGuid(),
			name,
			email,
			photoUrl,
			nickname,
			phoneNumber,
			pixKey,
			pixKeyType,
			createdAt: DateTime.Today)
	{
	}

	public static User CreateNew(
		string name,
		string email,
		string? photoUrl,
		string nickname,
		string phoneNumber,
		string pixKey,
		string pixKeyType)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), Messages.EnterUserEmail);

		if (string.IsNullOrWhiteSpace(nickname))
			throw new ArgumentNullException(nameof(nickname), Messages.EnterUserNickname);

		if (string.IsNullOrWhiteSpace(phoneNumber))
			throw new ArgumentNullException(nameof(phoneNumber), Messages.EnterUserPhoneNumber);

		if (string.IsNullOrWhiteSpace(pixKey))
			throw new ArgumentNullException(nameof(pixKey), Messages.EnterUserPixKey);

		if (string.IsNullOrWhiteSpace(pixKeyType))
			throw new ArgumentNullException(nameof(pixKeyType), Messages.EnterUserPixKeyType);

		return new User(
			name,
			email,
			photoUrl,
			nickname,
			phoneNumber,
			pixKey,
			pixKeyType);
	}

	public static User Restore(
		Guid id,
		string name,
		string email,
		string? photoUrl,
		string nickname,
		string phoneNumber,
		string pixKey,
		string pixKeyType,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), Messages.EnterUserEmail);

		if (string.IsNullOrWhiteSpace(nickname))
			throw new ArgumentNullException(nameof(nickname), Messages.EnterUserNickname);

		if (string.IsNullOrWhiteSpace(phoneNumber))
			throw new ArgumentNullException(nameof(phoneNumber), Messages.EnterUserPhoneNumber);

		if (string.IsNullOrWhiteSpace(pixKey))
			throw new ArgumentNullException(nameof(pixKey), Messages.EnterUserPixKey);

		if (string.IsNullOrWhiteSpace(pixKeyType))
			throw new ArgumentNullException(nameof(pixKeyType), Messages.EnterUserPixKeyType);

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterUserCreationDate);

		return new User(
			id,
			name,
			email,
			photoUrl,
			nickname,
			phoneNumber,
			pixKey,
			pixKeyType,
			createdAt);
	}

	public void UpdateName(string name)
	{
		if (!string.IsNullOrWhiteSpace(name))
			Name = name;
	}

	public void UpdateEmail(string email)
	{
		if (!string.IsNullOrWhiteSpace(email))
			Email = Email.Create(email);
	}

	public void UpdatePhoto(string photoUrl)
	{
		if (!string.IsNullOrWhiteSpace(photoUrl))
			Photo = Photo.Create(photoUrl);
	}

	public void UpdateNickname(string nickname)
	{
		if (!string.IsNullOrWhiteSpace(nickname))
			Nickname = nickname;
	}

	public void UpdatePhoneNumber(string phoneNumber)
	{
		if (!string.IsNullOrWhiteSpace(phoneNumber))
			PhoneNumber = PhoneNumber.Create(phoneNumber);
	}

	public void UpdatePixKey(string pixKey, string pixKeyType)
	{
		if (!string.IsNullOrWhiteSpace(pixKey) && !string.IsNullOrWhiteSpace(pixKeyType))
			PixKey = PixKey.Create(pixKey, pixKeyType);
	}
}
