namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class AddUserToPlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	private readonly DateTime _today = DateTime.Today;

	/*[Fact]
	public async Task Test_AddUserToPlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.Plan[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserIsAlreadyInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToRunningPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}

	[Fact]
	public async Task Test_AddUserToPlanUseCase_UserCannotBeAddedToCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<AddUserToPlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new AddUserToPlanCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		var resultData = Assert.IsType<Result<PlanParticipantModel>>(result).Data;
		Assert.Equal(firstPlanParticipant.Prop, resultData.Prop);
		Assert.Equal(firstPlanParticipant.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.CreatedAt);

		_mock.PlanRepository.Verify(repo => repo.zzzAsync(It.IsAny<Plan>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.zzzAsync(It.IsAny<PlanParticipant>()), Times.Once);
	}*/
}
