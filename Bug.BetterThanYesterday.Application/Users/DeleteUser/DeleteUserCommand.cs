using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.DeleteUser;

public class DeleteUserCommand : ICommand
{
	public DeleteUserCommand(Guid userId) => UserId = userId;

	public Guid UserId { get; init; }

	public void Validate()
	{
		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), "Informe o ID do usuário");
	}
}
