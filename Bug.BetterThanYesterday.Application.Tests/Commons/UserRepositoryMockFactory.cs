using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Users;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons
{
	public static class UserRepositoryMockFactory
	{
		public static Mock<IUserRepository> CreateDefault()
		{
			var userRepository = new Mock<IUserRepository>();
			
			var users = new List<User>
			{
				User.CreateNew("Fake User", "existing@email.com")
			};

			userRepository = new Mock<IUserRepository>();

			userRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()));

			userRepository
				.Setup(repo => repo.GetByEmailAsync(users[0].Email))
				.ReturnsAsync(users[0]);

			userRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(users[0]);

			userRepository
				.Setup(repo => repo.ListAllAsync())
				.ReturnsAsync(users);

			userRepository
				.Setup(repo => repo.UpdateAsync(It.IsAny<User>()));

			userRepository
				.Setup(repo => repo.DeleteAsync(It.IsAny<User>()));

			return userRepository;
		}
	}
}
