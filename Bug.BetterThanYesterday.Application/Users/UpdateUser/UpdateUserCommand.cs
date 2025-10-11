using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserCommand : ICommand
{
	public UpdateUserCommand(Guid userId, string name, string email)
	{
		UserId = userId;
		Name = name;
		Email = email;
	}

	public Guid UserId { get; init; }
	public string Name { get; init; }
	public string Email { get; init; }

	public void Validate()
	{
		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), "Informe o ID do usuário");

		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), "Informe o e-mail do usuário");
	}
}
