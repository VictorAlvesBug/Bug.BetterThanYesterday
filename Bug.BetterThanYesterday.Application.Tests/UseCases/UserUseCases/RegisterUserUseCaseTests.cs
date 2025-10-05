using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class RegisterUserUseCaseTests
{
	private readonly AutoMocker _mocker = new();
	private readonly Mock<IUserRepository> _userRepository;
	private readonly List<User> _users;

	public RegisterUserUseCaseTests()
	{
		(_userRepository, _users) = UserRepositoryMockFactory.Create();
		_mocker.Use(_userRepository.Object);
	}

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

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyName_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("", "jane.doe@email.com");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_EmptyEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_InvalidEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var command = new RegisterUserCommand("Jane Doe", "invalid_email");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_RegisterUserUseCase_DuplicatedEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<RegisterUserUseCase>();
		var firstUser = _users[0];
		var command = new RegisterUserCommand("Other Name", firstUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
