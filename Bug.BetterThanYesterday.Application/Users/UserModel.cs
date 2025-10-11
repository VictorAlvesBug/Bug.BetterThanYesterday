namespace Bug.BetterThanYesterday.Application.Users;

public class UserModel
{
	public Guid UserId { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime CreatedAt { get; set; }
}
