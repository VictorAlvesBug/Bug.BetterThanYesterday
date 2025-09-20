using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.DeleteUser;

public class DeleteUserCommand : ICommand
{
	public DeleteUserCommand(string id) => Id = id;

	public string Id { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");
	}
}
