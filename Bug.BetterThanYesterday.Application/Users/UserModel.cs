namespace Bug.BetterThanYesterday.Application.Users;

public class UserModel
{
	public Guid UserId { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string? PhotoUrl { get; set; }
	public string Nickname { get; set; }
	public string PhoneNumber { get; set; }
	public string PixKey { get; set; }
	public string PixKeyType { get; set; }
	public DateTime CreatedAt { get; set; }
}
