using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons
{
	public static class UserRepositoryMockFactory
	{
		public static (Mock<IUserRepository> repo, List<User> data) Create()
		{
			var userRepository = new Mock<IUserRepository>();

			List<User> users =
			[
				User.CreateNew("Ana", "ana@ex.com"),
				User.CreateNew("Bob", "bob@ex.com")
			];

			userRepository = new Mock<IUserRepository>();

			userRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()));

			userRepository
				.Setup(repo => repo.ListAllAsync())
				.ReturnsAsync(users);

			userRepository
				.Setup(repo => repo.UpdateAsync(It.IsAny<User>()));

			userRepository
				.Setup(repo => repo.DeleteAsync(It.IsAny<User>()));

			users.ForEach(user =>
			{
				userRepository
					.Setup(repo => repo.GetByIdAsync(user.Id))
					.ReturnsAsync(user);

				userRepository
					.Setup(repo => repo.GetByEmailAsync(user.Email))
					.ReturnsAsync(user);
			});

			return (userRepository, users);
		}
	}
}
