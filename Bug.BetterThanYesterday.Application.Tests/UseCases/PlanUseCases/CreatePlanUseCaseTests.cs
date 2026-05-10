using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class CreatePlanUseCaseTests : BasePlanUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_CreatePlanUseCase_PlanSuccessfullyRegistered_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanSuccessfullyRegistered, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;
		Assert.NotEqual(Guid.Empty, resultData.PlanId);
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
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Once);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_HabitNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var command = new CreatePlanCommand(
			owner.Id,
			Guid.NewGuid(),
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Name,
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
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EnterPlanStartDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			DateTime.MinValue,
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterPlanStartDate, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_StartDateCannotBeEarlierThanToday_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year - 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Name,
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
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EnterPlanEndDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			DateTime.MinValue,
			PlanType.Private.Name,
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterPlanEndDate, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EndDateMustBeLaterThanStartDate_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 2, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Name,
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
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EnterPlanType_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var owner = _mock.Users.First();
		var habit = _mock.Habits.First(habit => habit.Id == HabitRepositoryMockFactory.HabitId4);
		var command = new CreatePlanCommand(
			owner.Id,
			habit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			"Invalid planType",
			2,
			10
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Contains(Messages.EnterPlanType, result.GetMessage());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}
}
