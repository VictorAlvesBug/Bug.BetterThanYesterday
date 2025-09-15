using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users;

internal sealed class UserMapper : IModelMapper<User, UserModel>
{
	public UserModel ToModel(User user) => new()
	{
		Id = user.Id,
		Name = user.Name,
		Email = user.Email.Value,
		CreatedAt = user.CreatedAt,
	};
}
