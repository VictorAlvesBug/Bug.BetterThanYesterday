using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserCommand : ICommand
{
	public RegisterUserCommand(
		string name,
		string email)
	{
		Name = name;
		Email = email;
	}

	public string Name { get; init; }
	public string Email { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentNullException(nameof(Name), "Informe o nome do usuário");

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), "Informe o e-mail do usuário");
	}
}
