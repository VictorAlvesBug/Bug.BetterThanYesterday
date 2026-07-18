using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
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
		var command = new UpdateUserCommand(
			firstUser.Id,
			name: "Jane Doe",
			email: "jane.doe@email.com");

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
	public async Task Test_UpdateUserUseCase_NicknamePhonePhotoPixSuccessfullyUpdated_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(
			firstUser.Id,
			photoUrl: "https://example.com/photo.jpg",
			nickname: "NewNick",
			phoneNumber: "11987654321",
			pixKey: "11987654321",
			pixKeyType: "PhoneNumber");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyUpdated, result.GetMessage());

		var resultData = Assert.IsType<Result<UserModel>>(result).Data;
		Assert.Equal("NewNick", resultData.Nickname);
		Assert.Equal("11987654321", resultData.PhoneNumber);
		Assert.Equal("11987654321", resultData.PixKey);
		Assert.Equal("PhoneNumber", resultData.PixKeyType);
		Assert.Equal("https://example.com/photo.jpg", resultData.PhotoUrl);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<Email>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
	}

	[Fact]
	public async Task Test_UpdateUserUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var command = new UpdateUserCommand(Guid.NewGuid(), name: "Jane Doe", email: "jane.doe@email.com");

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
		var command = new UpdateUserCommand(firstUser.Id);

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
		var command = new UpdateUserCommand(firstUser.Id, name: "Other Name", email: otherUser.Email.Value);

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
		var command = new UpdateUserCommand(firstUser.Id, name: "Other Name", email: firstUser.Email.Value);

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
		var command = new UpdateUserCommand(firstUser.Id, name: "Jane Doe", email: "invalid_email");

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

	[Fact]
	public async Task Test_UpdateUserUseCase_InvalidPixKey_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UpdateUserUseCase>();
		var firstUser = _mock.Users[0];
		var command = new UpdateUserCommand(
			firstUser.Id,
			pixKey: "not-a-phone",
			pixKeyType: "PhoneNumber");

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.UserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
	}
}
