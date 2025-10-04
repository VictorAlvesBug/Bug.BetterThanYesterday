using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using NSubstitute;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class RegisterUserUseCaseTests
{
	private readonly AutoMocker _mocker = new();
	private Mock<IUserRepository> _userRepository;
	public RegisterUserUseCaseTests()
	{
		_userRepository = UserRepositoryMockFactory.CreateDefault();
		_mocker.Use(_userRepository.Object);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.GetRequiredService<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Victor Alves", "victor.alves@gmail.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.GetRequiredService<RegisterUserUseCase>();
		var command = new RegisterUserCommand("", "victor.alves@gmail.com");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.GetRequiredService<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Victor Alves", "");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_InvalidEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.GetRequiredService<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Victor Alves", "invalid_email");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_DuplicatedEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.GetRequiredService<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Other Name", "existing@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
