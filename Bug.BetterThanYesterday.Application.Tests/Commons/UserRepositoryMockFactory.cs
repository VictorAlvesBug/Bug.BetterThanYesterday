using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class UserRepositoryMockFactory
{
	public static readonly Guid UserId1 = Guid.Parse("57b8652a-81ad-46af-b50b-e1de389250da");
	public static readonly Guid UserId2 = Guid.Parse("814fbb49-66e1-4d51-a69e-bf1eb6d8fc4a");
	public static readonly Guid UserId3 = Guid.Parse("cc16329d-cbfc-4ef3-95bb-1b031179005f");
	public static readonly Guid UserId4 = Guid.Parse("78edf69e-bd58-4117-899d-be9150252d25");
	
	public static (Mock<IUserRepository> repo, List<User> data) Create()
	{
		List<User> users =
		[
			User.Restore(
				UserId1,
				"Ana",
				"ana@ex.com",
				new DateTime(2023, 06, 20)
			),
			User.Restore(
				UserId2,
				"Bob",
				"bob@ex.com",
				new DateTime(2024, 01, 10)
			),
			User.Restore(
				UserId3,
				"Carl",
				"carl@ex.com",
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId4,
				"David",
				"david@ex.com",
				new DateTime(2020, 06, 20)
			)
		];

		var userRepository = new Mock<IUserRepository>();

		userRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()));

		userRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(users);

		userRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<User>()));

		userRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<User>()));

		userRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid userId) => users
				.Find(user => user.Id == userId));

		userRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> userIds) => users
				.Where(user => userIds.Contains(user.Id))
				.ToList());

		userRepository
			.Setup(repo => repo.GetByEmailAsync(It.IsAny<Email>()))
			.ReturnsAsync((Email email) => users
				.Find(user => user.Email == email));

		return (userRepository, users);
	}
}
