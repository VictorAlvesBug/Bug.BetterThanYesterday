namespace Bug.BetterThanYesterday.Application.Users
{
	public class UserModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateOnly CreatedAt { get; set; }
	}
}
