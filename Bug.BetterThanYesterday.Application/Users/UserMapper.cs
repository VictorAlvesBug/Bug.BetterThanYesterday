using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users;

internal static class UserMapper
{
	public static UserModel ToModel(this User user) => new()
	{
		Id = user.Id,
		Name = user.Name,
		Email = user.Email.Value,
		PhotoUrl = user.Photo is null ? null : user.Photo.Value,
		Nickname = user.Nickname,
		PhoneNumber = user.PhoneNumber.Value,
		PixKey = user.PixKey.Value,
		PixKeyType = user.PixKey.GetPixKeyType(),
		CreatedAt = user.CreatedAt
	};
}
