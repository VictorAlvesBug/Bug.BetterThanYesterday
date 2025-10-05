using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class UpdateUserUseCaseTests
{
	private readonly AutoMocker _mocker = new();
	private readonly Mock<IUserRepository> _userRepository;
	private readonly List<User> _users;

	public UpdateUserUseCaseTests()
	{
		(_userRepository, _users) = UserRepositoryMockFactory.Create();
		_mocker.Use(_userRepository.Object);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Jane Doe", "jane.doe@email.com");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
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

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_EmptyNameAndEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _users[0];
		var command = new UpdateUserCommand(firstUser.Id, "", "");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_SameEmailAndOtherId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _users[0];
		var otherUser = _users[1];
		var command = new UpdateUserCommand(firstUser.Id, "Other Name", otherUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_SameEmailAndSameId_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Other Name", firstUser.Email.Value);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
		_userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_InvalidEmail_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _users[0];
		var command = new UpdateUserCommand(firstUser.Id, "Jane Doe", "invalid_email");

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_userRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
	}
}
