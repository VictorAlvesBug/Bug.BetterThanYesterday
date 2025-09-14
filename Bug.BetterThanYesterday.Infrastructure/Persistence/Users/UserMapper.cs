using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users
{
	internal sealed class UserMapper : IDocumentMapper<User, UserDocument>
	{
		public UserDocument ToDocument(User user) => new()
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email.Value,
			CreatedAt = user.CreatedAt,
		};

		public User ToDomain(UserDocument document) => User.Restore(
			document.Id,
			document.Name,
			Email.Create(document.Email),
			document.CreatedAt);
	}
}
