using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.DeleteUser;

public class DeleteUserCommand : ICommand
{
	public DeleteUserCommand(Guid id) => Id = id;

	public Guid Id { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");
	}
}
