using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Users.ListUsersByFilter;

public class ListUsersByFilterCommand : ICommand
{
	public ListUsersByFilterCommand()
	{
	}
	public ListUsersByFilterCommand(string email) => Email = email;

	public string? Email { get; init; }

	public void Validate()
	{
	}
}
