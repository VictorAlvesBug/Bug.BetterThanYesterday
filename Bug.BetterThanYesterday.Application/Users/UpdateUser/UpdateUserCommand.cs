using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

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
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

		if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Email))
			throw new ArgumentException(Messages.EnterUserNameOrEmail);
	}
}
