using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users;

internal static class UserMapper
{
	public static UserModel ToModel(this User user) => new()
	{
		Id = user.Id,
		Name = user.Name,
		Email = user.Email.Value,
		CreatedAt = user.CreatedAt.ToDateTime(TimeOnly.MinValue),
	};
}
