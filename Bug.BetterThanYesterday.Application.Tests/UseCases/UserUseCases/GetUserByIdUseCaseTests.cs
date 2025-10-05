using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using MongoDB.Bson.Serialization.IdGenerators;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases
{
	public class GetUserByIdUseCaseTests
	{
		private readonly AutoMocker _mocker = new();
		private Mock<IUserRepository> _userRepository;
		private readonly List<User> _users;

		public GetUserByIdUseCaseTests()
		{
			(_userRepository, _users) = UserRepositoryMockFactory.Create();
			_mocker.Use(_userRepository.Object);
		}

		[Fact]
		public async Task Test_GetUserByIdUseCase_Valid_ShouldReturnSuccess()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<GetUserByIdUseCase>();
			var firstUser = _users[0];
			var command = new GetUserByIdCommand(firstUser.Id);

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsSuccess());

			var resultData = Assert.IsType<Result<UserModel>>(result).Data;
			Assert.Equal(firstUser.Id, resultData.Id);
			Assert.Equal(firstUser.Name, resultData.Name);
			Assert.Equal(firstUser.Email.Value, resultData.Email);
			Assert.Equal(firstUser.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

			_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}

		[Fact]
		public async Task Test_GetUserByIdUseCase_NotFoundUserId_ShouldReturnRejected()
		{
			// Arrange
			var useCase = _mocker.CreateInstance<GetUserByIdUseCase>();
			var command = new GetUserByIdCommand(Guid.NewGuid());

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsRejected());

			_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		}
	}
}
