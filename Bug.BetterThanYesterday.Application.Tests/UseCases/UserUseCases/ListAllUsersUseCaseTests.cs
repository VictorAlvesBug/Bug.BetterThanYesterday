using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class ListAllUsersUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_ListAllUsersUseCase_UsersSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListAllUsersUseCase>();
		var command = new ListAllUsersCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UsersSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<UserModel>>>(result).Data;
		Assert.Equal(_mock.Users.Count, resultData.Count());

		_mock.UserRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
	}
}
