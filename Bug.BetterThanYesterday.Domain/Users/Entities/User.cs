using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Users.Entities;

public class User : Entity
{
	public string Name { get; set; }
	public Email Email { get; set; }
	public DateOnly CreatedAt { get; set; }

	private User(
		Guid id,
		string name,
		string email,
		DateTime createdAt)
	{
		Id = id;
		Name = name;
		Email = Email.Create(email);
		CreatedAt = DateOnly.FromDateTime(createdAt);
	}

	private User(string name, string email)
		: this(
			id: Guid.NewGuid(),
			name,
			email,
			createdAt: DateTime.Today)
	{
	}

	public static User CreateNew(string name, string email)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), Messages.EnterUserEmail);

		return new User(name, email);
	}

	public static User Restore(
		Guid id,
		string name,
		string email,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), Messages.EnterUserEmail);

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), Messages.EnterUserCreationDate);

		return new User(
			id,
			name,
			email,
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
}
