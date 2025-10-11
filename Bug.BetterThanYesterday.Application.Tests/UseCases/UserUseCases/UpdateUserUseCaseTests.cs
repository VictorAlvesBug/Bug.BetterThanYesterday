using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class UpdateUserUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_UpdateUserUseCase_Valid_ShouldReturnSuccess()
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

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var command = new UpdateUserCommand(Guid.NewGuid(), "Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_EmptyNameAndEmail_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, string.Empty, string.Empty);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_SameEmailAndOtherId_ShouldReturnRejected()
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

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_SameEmailAndSameId_ShouldReturnSuccess()
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

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_InvalidEmail_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Jane Doe", "invalid_email");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
