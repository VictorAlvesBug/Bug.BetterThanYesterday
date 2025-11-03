using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class RegisterUserUseCaseTests : BaseUserUseCaseTests
{
	[Fact]
	public async Task Test_RegisterUserUseCase_UserSuccessfullyRegistered_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyRegistered, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EnterUserName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand(string.Empty, "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterUserName, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EnterUserEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", string.Empty);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterUserEmail, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EnterValidUserEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "invalid_email");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterValidUserEmail, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_UserEmailAlreadyRegistered_ShouldReturnRejected()
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
		Assert.Equal(Messages.UserEmailAlreadyRegistered, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
