using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.GetUserById;

public class GetUserByIdCommand : ICommand
{
	public GetUserByIdCommand(Guid id) => Id = id;

	public Guid Id { get; init; }

	public void Validate()
	{
		if (Id == Guid.Empty)
			throw new ArgumentNullException(nameof(Id), "Informe o ID do usuário");
	}
}
