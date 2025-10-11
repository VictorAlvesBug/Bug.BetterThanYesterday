using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans.Entities;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class CreatePlanUseCaseTests : BasePlanUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	[Fact]
	public async Task Test_CreatePlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanModel>>(result).Data;
		Assert.NotEqual(Guid.Empty, resultData.PlanId);
		Assert.Equal(command.HabitId, resultData.HabitId);
		Assert.Equal(command.Description, resultData.Description);
		Assert.Equal(command.StartsAt, resultData.StartsAt);
		Assert.Equal(command.EndsAt, resultData.EndsAt);
		Assert.Equal(command.TypeId, resultData.TypeId);
		Assert.Equal(_today, resultData.CreatedAt);

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Once);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_NotFoundHabitId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var command = new CreatePlanCommand(
			Guid.NewGuid(),
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Id
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EmptyStartsAt_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			DateTime.MinValue,
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Id
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_StartsAtBeforeToday_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year - 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Id
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EmptyEndsAt_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			DateTime.MinValue,
			PlanType.Private.Id
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EndsAtBeforeStartsAt_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 2, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			PlanType.Private.Id
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_EmptyTypeId_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			0
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}

	[Fact]
	public async Task Test_CreatePlanUseCase_InvalidTypeId_ShouldThrowsException()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<CreatePlanUseCase>();
		var cookingHabit = _mock.Habits.First(habit => habit.Name == "Cooking");
		var command = new CreatePlanCommand(
			cookingHabit.Id,
			"Prepare a meal once a week",
			new DateTime(_today.Year + 1, 01, 01),
			new DateTime(_today.Year + 1, 12, 31),
			-1
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.HandleAsync(command));

		_mock.HabitRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.AddAsync(It.IsAny<Plan>()), Times.Never);
	}
}
