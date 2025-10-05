using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases
{
	public class ListAllUsersUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IUserRepository> _userRepository;
		private readonly List<User> _users;

		public ListAllUsersUseCaseTests()
		{
			(_userRepository, _users) = UserRepositoryMockFactory.Create();
			_mocker.Use(_userRepository.Object);
		}

		[Fact]
		public async Task Test_ListAllUsersUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<ListAllUsersUseCase>();
			var command = new ListAllUsersCommand();

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			var resultData = Assert.IsType<Result<IEnumerable<UserModel>>>(result).Data;
			Assert.Equal(_users.Count, resultData.Count());

			_userRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
		}
	}
}
