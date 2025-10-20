using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

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
			throw new ArgumentNullException(nameof(Name), Messages.EnterUserName);

		if (string.IsNullOrWhiteSpace(Email))
			throw new ArgumentNullException(nameof(Email), Messages.EnterUserEmail);
	}
}
