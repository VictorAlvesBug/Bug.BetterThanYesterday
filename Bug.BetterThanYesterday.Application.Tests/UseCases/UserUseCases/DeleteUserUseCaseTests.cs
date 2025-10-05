using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.DeleteUser;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases
{
	public class DeleteUserUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IUserRepository> _userRepository;
		private readonly List<User> _users;

		public DeleteUserUseCaseTests()
		{
			(_userRepository, _users) = UserRepositoryMockFactory.Create();
			_mocker.Use(_userRepository.Object);
		}

		[Fact]
		public async Task Test_DeleteUserUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<DeleteUserUseCase>();
			var firstUser = _users[0];
			var command = new DeleteUserCommand(firstUser.Id);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}

		[Fact]
		public async Task Test_DeleteUserUseCase_NotFoundUserId_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<DeleteUserUseCase>();
			var command = new DeleteUserCommand(Guid.NewGuid());

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}
	}
}
