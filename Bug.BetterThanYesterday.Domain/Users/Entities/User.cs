using Bug.BetterThanYesterday.Domain.Commons;
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
			throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), "Informe o e-mail do usuário");

		return new User(name, email);
	}

	public static User Restore(
		Guid id,
		string name,
		string email,
		DateTime createdAt)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), "Informe o ID do usuário");

		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentNullException(nameof(email), "Informe o e-mail do usuário");

		if (createdAt == DateTime.MinValue)
			throw new ArgumentNullException(nameof(createdAt), "Informe a data de criação do usuário");

		return new User(
			id,
			name,
			email,
			createdAt);
	}

	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

		Name = name;
	}

	public void UpdateEmail(string email)
	{
		Email = Email.Create(email);
	}
}
