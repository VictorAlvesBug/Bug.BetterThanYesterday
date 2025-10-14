namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class UnblockUserInThePlanUseCaseTests : BasePlanParticipantUseCaseTests
{
	/*[Fact]
	public async Task Test_UnblockUserInThePlanUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new UnblockUserInThePlanCommand();

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
	public async Task Test_UnblockUserInThePlanUseCase_NotFoundPlanId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new UnblockUserInThePlanCommand();

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
	public async Task Test_UnblockUserInThePlanUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new UnblockUserInThePlanCommand();

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
	public async Task Test_UnblockUserInThePlanUseCase_UserIsNotInThePlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new UnblockUserInThePlanCommand();

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
	public async Task Test_UnblockUserInThePlanUseCase_UserIsNotBlocked_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<UnblockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new UnblockUserInThePlanCommand();

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
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInNotStartedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new BlockUserInThePlanCommand();

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
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInFinishedPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new BlockUserInThePlanCommand();

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
	public async Task Test_BlockUserInThePlanUseCase_UserCannotBeUnblockedInCancelledPlan_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<BlockUserInThePlanUseCase>();
		var firstPlanParticipant = _mock.PlanParticipants[0];
		var command = new BlockUserInThePlanCommand();

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
