using Bug.BetterThanYesterday.Domain.Commons;
using System.Text.RegularExpressions;

namespace Bug.BetterThanYesterday.Domain.Users.Entities
{
	public class User : Entity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public DateOnly CreatedAt { get; set; }

		public User(string name, string email)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Informe o nome do usuário");

			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentNullException(nameof(email), "Informe o e-mail do usuário");

			var regex = new Regex(@"^[a-z0-9+_.-]+@([a-z0-9-]+\.)+[a-z]{2,6}$");
			if (!regex.IsMatch(email))
				throw new ArgumentException(nameof(email), "Informe um e-mail válido para o usuário");

			Id = Guid.NewGuid().ToString();
			Name = name;
			Email = email;
			CreatedAt = DateOnly.FromDateTime(DateTime.Today);
		}
	}
}
