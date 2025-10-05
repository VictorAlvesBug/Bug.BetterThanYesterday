using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class UserRepositoryMockFactory
{
	public static (Mock<IUserRepository> repo, List<User> data) Create()
	{
		List<User> users =
		[
			User.Restore(
				Guid.Parse("57b8652a-81ad-46af-b50b-e1de389250da"),
				"Ana",
				"ana@ex.com",
				new DateTime(2023, 06, 20)
			),
			User.Restore(
				Guid.Parse("814fbb49-66e1-4d51-a69e-bf1eb6d8fc4a"),
				"Bob",
				"bob@ex.com",
				new DateTime(2024, 01, 10)
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
			.ReturnsAsync((Guid userId) => users.Find(user => user.Id == userId));

		userRepository
			.Setup(repo => repo.GetByEmailAsync(It.IsAny<Email>()))
			.ReturnsAsync((Email email) => users.Find(user => user.Email == email));

		return (userRepository, users);
	}
}
