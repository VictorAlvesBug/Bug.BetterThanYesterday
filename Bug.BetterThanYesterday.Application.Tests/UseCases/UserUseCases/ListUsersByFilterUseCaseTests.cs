using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.ListUsersByFilter;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class ListUsersByFilterUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_ListUsersByFilterUseCase_UsersSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListUsersByFilterUseCase>();
		var command = new ListUsersByFilterCommand();

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
