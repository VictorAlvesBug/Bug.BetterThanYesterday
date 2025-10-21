using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class GetUserByIdUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_GetUserByIdUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserByIdUseCase>();
		var firstUser = _mock.Users[0];
		var command = new GetUserByIdCommand(firstUser.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<UserModel>>(result).Data;
		Assert.Equal(firstUser.Id, resultData.UserId);
		Assert.Equal(firstUser.Name, resultData.Name);
		Assert.Equal(firstUser.Email.Value, resultData.Email);
		Assert.Equal(firstUser.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
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
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}
}
