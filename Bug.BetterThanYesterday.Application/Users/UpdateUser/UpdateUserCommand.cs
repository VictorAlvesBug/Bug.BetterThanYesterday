using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserCommand : ICommand
{
	public UpdateUserCommand(Guid id, string name, string email)
	{
		Id = id;
		Name = name;
		Email = email;
	}

	public Guid Id { get; init; }
	public string Name { get; init; }
	public string Email { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), "Informe o e-mail do usuário");
	}
}
