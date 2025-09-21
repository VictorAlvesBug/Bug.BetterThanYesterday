using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users;

public class UserDocument : Document
{
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime CreatedAt { get; set; }
}
