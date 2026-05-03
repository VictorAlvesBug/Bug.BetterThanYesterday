using Bug.BetterThanYesterday.Application.SeedWork;

namespace Bug.BetterThanYesterday.Application.Users.ListUsersByFilter;

public class ListUsersByFilterCommand : ICommand
{
	public string? Email { get; init; }

	public void Validate()
	{
	}
}
