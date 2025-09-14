using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Users.Entities
{
	public class User : Entity
	{
		public string Name { get; set; }
		public Email Email { get; set; }
		public DateOnly CreatedAt { get; set; }

		private User(string name, string email)
		{
			Id = Guid.NewGuid().ToString();
			Name = name;
			Email = Email.Create(email);
			CreatedAt = DateOnly.FromDateTime(DateTime.Today);
		}

		public static User CreateNew(string name, string email)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

			return new User(name, email);
		}
	}
}
