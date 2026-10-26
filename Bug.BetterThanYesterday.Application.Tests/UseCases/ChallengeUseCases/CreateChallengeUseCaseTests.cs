using Bug.BetterThanYesterday.Application.Challenges.CreateChallenge;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Application.Mocks;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class CreateChallengeUseCaseTests : BaseChallengeUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_CreateChallengeUseCase_ChallengeSuccessfullyRegistered_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.ChallengeSuccessfullyRegistered, result.GetMessage());

		var resultData = Assert.IsType<Result<ChallengeModel>>(result).Data;
		Assert.NotEqual(Guid.Empty, resultData.Id);
		Assert.Equal(command.OwnerId, resultData.OwnerId);
		Assert.Equal(command.HabitId, resultData.HabitId);
		Assert.Equal(command.Description, resultData.Description);
		Assert.Equal(command.StartsAt, resultData.StartsAt);
		Assert.Equal(command.EndsAt, resultData.EndsAt);
		Assert.Equal(command.Type, resultData.Type);
		Assert.Equal(command.DaysOffPerWeek, resultData.DaysOffPerWeek);
		Assert.Equal(command.PenaltyValue, resultData.PenaltyValue);
		Assert.Equal(_today, resultData.CreatedAt);

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Once);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var command = new CreateChallengeCommand(
			owner.Id,
			Guid.NewGuid(),
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.HabitNotFound, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_EnterChallengeStartDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			DateTime.MinValue,
			new DateTime(_today.Year + 1, 12, 31),
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterChallengeStartDate, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_StartDateCannotBeEarlierThanToday_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year - 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.StartDateCannotBeEarlierThanToday, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_EnterChallengeEndDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			DateTime.MinValue,
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterChallengeEndDate, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_EndDateMustBeLaterThanStartDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 2, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			ChallengeType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EndDateMustBeLaterThanStartDate, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreateChallengeUseCase_EnterChallengeType_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreateChallengeUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == MockData.HabitId4);
		var command = new CreateChallengeCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			"Invalid challengeType",
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterChallengeType, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.ChallengeRepository.Verify(repo => repo.AddAsync(It.IsAny<Challenge>()), Times.Never);
	}
}
