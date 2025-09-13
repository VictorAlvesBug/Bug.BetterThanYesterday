namespace Bug.BetterThanYesterday.Domain.Users.Entities
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateOnly CreatedAt { get; set; }

		public User(string name, string email) {
			Id = Guid.NewGuid().ToString();
			Name = name;
			Email = email;
			CreatedAt = DateOnly.FromDateTime(DateTime.Today);
		}
	}
}
