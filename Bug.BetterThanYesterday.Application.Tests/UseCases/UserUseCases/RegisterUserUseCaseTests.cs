using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class RegisterUserUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_RegisterUserUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyName_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand(string.Empty, "jane.doe@email.com");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyEmail_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", string.Empty);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_InvalidEmail_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "invalid_email");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_DuplicatedEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new RegisterUserCommand("Other Name", firstUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
