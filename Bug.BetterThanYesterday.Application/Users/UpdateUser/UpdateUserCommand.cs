using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserCommand : ICommand
{
	public UpdateUserCommand(string id, string name, string email)
	{
		Id = id;
		Name = name;
		Email = email;
	}

	public string Id { get; }
	public string Name { get; }
	public string Email { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), "Informe o e-mail do usuário");
	}
}
