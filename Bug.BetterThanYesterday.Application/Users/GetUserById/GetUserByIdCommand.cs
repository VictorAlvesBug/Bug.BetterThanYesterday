using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.GetUserById;

public class GetUserByIdCommand : ICommand
{
	public GetUserByIdCommand(string id) => Id = id;

	public string Id { get; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Id))
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");
	}
}
