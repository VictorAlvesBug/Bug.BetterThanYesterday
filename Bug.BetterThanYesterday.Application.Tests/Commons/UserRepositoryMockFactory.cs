using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class UserRepositoryMockFactory
{
	public static (Mock<IUserRepository> repo, List<User> data) Create()
	{
		List<User> users = MockData.MockUsers;

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
