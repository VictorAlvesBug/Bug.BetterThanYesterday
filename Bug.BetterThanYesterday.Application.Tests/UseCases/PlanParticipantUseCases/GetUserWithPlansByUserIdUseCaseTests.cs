using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class GetUserWithPlansByUserIdUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_UserSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var userId = _mock.PlanParticipants.First().UserId;
		var user = _mock.Users.First(user => user.Id == userId);
		var plans =
			(
				from
					planParticipant in _mock.PlanParticipants
				where
					planParticipant.UserId == userId
				join plan in _mock.Plans 
					on planParticipant.PlanId equals plan.Id
				select
					plan
			).Distinct()
			.ToList();
		var command = new GetUserWithPlansByUserIdCommand(userId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<UserWithPlansModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.UserId);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.User.CreatedAt);
		
		Assert.Equal(plans.Count, resultData.Plans.Count);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_UserHasNoPlans_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var user = _mock.Users.First(user => user.Id == UserRepositoryMockFactory.UserId6);
		var command = new GetUserWithPlansByUserIdCommand(user.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.UserHasNoPlans, result.GetMessage());

		var resultData = Assert.IsType<Result<UserWithPlansModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.UserId);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.User.CreatedAt);
		
		Assert.Empty(resultData.Plans);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_UserNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var command = new GetUserWithPlansByUserIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.UserNotFound, result.GetMessage());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
