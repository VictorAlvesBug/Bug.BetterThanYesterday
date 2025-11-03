using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class UpdateUserUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_UpdateUserUseCase_UserSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyUpdated, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var command = new UpdateUserCommand(Guid.NewGuid(), "Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_EnterUserNameOrEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, string.Empty, string.Empty);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterUserNameOrEmail, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_UserEmailAlreadyRegistered_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var otherUser = _mock.Users[1];
		var command = new UpdateUserCommand(firstUser.Id, "Other Name", otherUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserEmailAlreadyRegistered, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_UserWithSameEmailSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Other Name", firstUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyUpdated, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_EnterValidUserEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Jane Doe", "invalid_email");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterValidUserEmail, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
