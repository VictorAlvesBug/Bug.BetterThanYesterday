using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Users.Entities;

public class User : Entity
{
	public string Name { get; set; }
	public Email Email { get; set; }
	public DateOnly CreatedAt { get; set; }

	private User(
		string id,
		string name,
		Email email,
		DateOnly createdAt)
	{
		Id = id;
		Name = name;
		Email = email;
		CreatedAt = createdAt;
	}

	private User(string name, string email)
		: this(
			id: Guid.NewGuid().ToString(),
			name,
			Email.Create(email),
			createdAt: DateOnly.FromDateTime(DateTime.Today))
	{
	}

	public static User CreateNew(string name, string email)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

		return new User(name, email);
	}

	public static User Restore(
		string id,
		string name,
		Email email,
		DateOnly createdAt)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentNullException(nameof(id), "Informe o ID do usuário");

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

		return new User(
			id,
			name,
			email,
			createdAt);
	}
}
